using System.Drawing;
using HidApi;
using KeychronLegacyLighting.Exceptions;

namespace KeychronLegacyLighting
{
    public class RgbController : IDisposable
    {
        private Device? hidDevice = null;
        private Keyboard keyboard;
        private Color[] colors;

        private const int PacketDataLength = 64;
        private const byte PacketHeader = 0x04;
        private const byte LedSpecialEffectPackets = 0x12;
        private const byte ReportId = 0x00;
        private const byte EffectPageLength = 16;
        private const byte EffectPageCheckCodeL = 0xAA;
        private const byte EffectPageCheckCodeH = 0x55;
        private const int ColorBufSize = 576;

        public RgbController(Keyboard keyboard)
        {
            this.keyboard = keyboard;
            this.colors = new Color[keyboard.LedsCount];
        }

        public void Dispose()
        {
            Hid.Exit();

            if ( hidDevice != null )
            {
                hidDevice.Dispose();
            }            
        }

        public static Keyboard[] GetConnectedDevices()
        {
            return Hid
                .Enumerate(Keyboard.K2.VendorId, Keyboard.K2.ProductId)
                .Where(x => x.Path.Contains(Keyboard.K2.HIDPathPrefix))
                .Select(x => Keyboard.K2)
                .ToArray();
        }

        public bool Connect()
        {
            var deviceInfo = Hid
                .Enumerate(keyboard.VendorId, keyboard.ProductId)
                .FirstOrDefault(x => x.Path.Contains(keyboard.HIDPathPrefix));

            Console.WriteLine(keyboard.HIDPathPrefix);
            Console.WriteLine();

            foreach (var item in Hid.Enumerate(keyboard.VendorId, keyboard.ProductId))
            {
                Console.WriteLine(item.Path);
            }

            if (deviceInfo == null)
            {
                return false;
            }

            hidDevice = deviceInfo.ConnectToDevice();
            return true;
        }

        public void SetColors(Color[] colors)
        {
            this.colors = colors;
        }

        public void SetColor(int index, Color color)
        {
            this.colors.SetValue(color, index);
        }

        public void SetMode(Mode mode)
        {
            if (hidDevice == null)
            {
                throw new KeychronDeviceException($"Device are not connected: {keyboard.Name}");
            }

            var modesCount = Mode.DefaultModes.Length;
            var modes = new Mode[modesCount];

            Array.Copy(Mode.DefaultModes, modes, modesCount);

            var selectedMode = modes.Single(x => x.Value == mode.Value);

            selectedMode = mode;

            SetCustomization(selectedMode.Value == Modes.CUSTOM);
            StartEffectPage();

            var usbBuf = new byte[PacketDataLength];
            var selectedModeBuf = new byte[EffectPageLength];
            /*-----------------------------------------*\
            |  Configure the modes                      |
            |  LED Effect Page structure:               |
            |                                           |
            |  OK.. this was from the original PDF      |
            |  which appears to not be exact/up to date |
            |-------------------------------------------|
            | [0] Specialeffects mode1-32               |
            | [1] colorFull color: 0x00 Monochrome:0x01 |
            | [2] R Color ratio 0x00-0xFF               |
            | [3] G Color Ratio0x00-0xFF                |
            | [4] B Colour ratio0x00-0xFF               |
            |       full color is 0,invalid             |
            | [5] dynamicdirection                      |
            |       left to right: 0x00                 |
            |       right to left: 0x01                 |
            |       down to up:    0x02                 |
            |       up to down:    0x03                 |
            | [6] brightnesscontrol 0x00-0x0F           |
            |       0x0F brightest                      |
            | [7] Periodiccontrol0x00-0x0F              |
            |       0x0F longest cycle                  |
            | [8:13] Reserved                           |
            | [14] Checkcode_L0xAA                      |
            | [15] Checkcode_H0x55                      |
            |-------------------------------------------|
            | Fixes:                                    |
            |  color mode is    8th byte                |
            |  brightness is    9th byte                |
            |  speed is         10th byte               |
            |  direction is     11th byte               |
            \*-----------------------------------------*/
            
            for (uint i = 0; i < 5; i++) // 5 packets
            {
                Array.Fill<byte>(usbBuf, 0x00);

                for (uint j = 0; j < 4; j++) // of 4 effects
                {
                    var modeToLoad = modes[1 + j + i * 4]; // skip 1 first mode (Custom)

                    uint offset = j * EffectPageLength;

                    usbBuf[offset + 0] = (byte)modeToLoad.Value;

                    if (modeToLoad.Flags.HasFlag(ModeFlags.HAS_MODE_SPECIFIC_COLOR))
                    {
                        usbBuf[offset + 1] = modeToLoad.SpecificColor.R;
                        usbBuf[offset + 2] = modeToLoad.SpecificColor.G;
                        usbBuf[offset + 3] = modeToLoad.SpecificColor.B;
                    }
                    usbBuf[offset + 8] = modeToLoad.ColorMode == ColorMode.RANDOM ? byte.MaxValue : byte.MinValue;
                    usbBuf[offset + 9] = modeToLoad.Brightness;
                    usbBuf[offset + 10] = modeToLoad.Speed;
                    usbBuf[offset + 11] = modeToLoad.Direction;

                    usbBuf[offset + 14] = EffectPageCheckCodeL;
                    usbBuf[offset + 15] = EffectPageCheckCodeH;

                    //  Backup active mode values for later use 
                    //  Custom and off share the same mode value 
                    if (modeToLoad.Value == selectedMode.Value
                        || modeToLoad.Value == Modes.LIGHTS_OFF && selectedMode.Value == Modes.CUSTOM)
                    {
                        usbBuf[offset + 9] = selectedMode.Brightness;

                        //for (uint x = 0; x < EffectPageLength; x++)
                        //{
                        //    selectedModeBuf[x] = usbBuf[offset + x];

                        //}

                        Array.Copy(usbBuf, offset, selectedModeBuf, 0, EffectPageLength);
                    }
                }

                Send(usbBuf);
            } // packets count sent: 5

            // 3 times an empty packet - guess why...   
            for (uint i = 0; i < 3; i++)
            {
                Array.Fill<byte>(usbBuf, 0x00);
                Send(usbBuf);
            } // packets count sent: 8

            // Customization stuff: 9 times * 16 blocks 80 RR GG BB 
            var colorBuf = Enumerable.Repeat<byte>(0x00, ColorBufSize).ToArray();

            for (uint i = 0; i < colors.Length; i++)
            {
                int offset = keyboard.LedsSequencePositions[i] * 4;

                colorBuf[offset + 0] = 0x80;
                colorBuf[offset + 1] = colors[i].R;
                colorBuf[offset + 2] = colors[i].G;
                colorBuf[offset + 3] = colors[i].B;
            }

            for (uint p = 0; p < 9; p++)
            {
                //memcpy(usb_buf, &color_buf[p * PACKET_DATA_LENGTH], PACKET_DATA_LENGTH);
                Array.Copy(colorBuf, p * PacketDataLength, usbBuf, 0, PacketDataLength);
                Send(usbBuf);
            } // packets count sent:  17

            // Tells the device what the active mode is. This is the last packet.                  
            Array.Fill<byte>(usbBuf, 0x00);
            Array.Copy(selectedModeBuf, 0, usbBuf, 0, EffectPageLength);

            Send(usbBuf); // packets count sent: 18 - let's hope the keyboard ACK in next frame

            EndCommunication();
            StartEffectCommand();
        }

        /// <summary>
        /// Tells the device to apply what we've sent
        /// </summary>
        private void StartEffectCommand()
        {
            byte[] usb_buf = new byte[PacketDataLength];

            Array.Fill<byte>(usb_buf, 0x00);

            usb_buf[0x00] = PacketHeader;
            usb_buf[0x01] = (byte)Commands.LED_EFFECT_START;

            Send(usb_buf);
        }

        /// <summary>
        /// Tells the device we're about to send the pages(18 pages)
        /// </summary>
        private void StartEffectPage()
        {
            // LED_SPECIAL_EFFECT_PACKETS: Packet amount that will be sent in this transaction
            byte[] usb_buf = new byte[PacketDataLength];
            Array.Fill<byte>(usb_buf, 0x00);

            usb_buf[0x00] = PacketHeader;
            usb_buf[0x01] = (byte)Commands.WRITE_LED_SPECIAL_EFFECT_AREA;
            usb_buf[0x08] = LedSpecialEffectPackets;

            Send(usb_buf);

            Read();
        }

        /// <summary>
        /// Turn customization on/off. Custom mode needs to turn it on.
        /// </summary>
        /// <param name="state"></param>
        private void SetCustomization(bool state)
        {
            byte[] usb_buf = new byte[PacketDataLength];

            Array.Fill<byte>(usb_buf, 0x00);

            usb_buf[0x00] = PacketHeader;
            usb_buf[0x01] = state ? (byte)Commands.TURN_ON_CUSTOMIZATION : (byte)Commands.TURN_OFF_CUSTOMIZATION;
            Send(usb_buf);

            Read();
        }

        /// <summary>
        /// Tells the device that the pages are sent
        /// </summary>
        private void EndCommunication()
        {
            byte[] usb_buf = new byte[PacketDataLength];

            Array.Fill<byte>(usb_buf, 0x00);

            usb_buf[0x00] = PacketHeader;
            usb_buf[0x01] = (byte)Commands.COMMUNICATION_END;

            Send(usb_buf);

            Read();
        }

        private ReadOnlySpan<byte> Read()
        {
            var report = hidDevice.GetFeatureReport(ReportId, PacketDataLength + 1);

            Thread.Sleep(1);

            return report;
        }

        private void Send(byte[] data)
        {
            byte[] usb_buf = new byte[PacketDataLength + 1];

            usb_buf[0] = ReportId;

            for (uint x = 0; x < PacketDataLength; x++)
            {
                usb_buf[x + 1] = data[x];
            }

            hidDevice.SendFeatureReport(usb_buf);
            Thread.Sleep(1);
        }
    }
}

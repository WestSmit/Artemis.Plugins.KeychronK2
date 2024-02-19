namespace KeychronLegacyLighting
{
    public class Keyboard
    {
        public required string Name { get; set; }
        public required ushort VendorId { get; set; }
        public required ushort ProductId { get; set; }
        public required string HIDPathPrefix { get; set; }
        public required int[] LedsSequencePositions { get; set; }
        public required int LedsCount { get; set; }
        public byte MinSpeed { get; set; }
        public byte MaxSpeed { get; set; }
        public byte MinBrightness { get; set; }
        public byte MaxBrightness { get; set; }

        public static Keyboard Default { get; } = new Keyboard()
        {
            Name = "Keychron Default",
            VendorId = 0x05ac,
            ProductId = 0x024f,
            HIDPathPrefix = "\\\\?\\HID#VID_05AC&PID_024F&MI_00",
            MinSpeed = 0x00,
            MaxSpeed = 0x0F,
            MinBrightness = 0x00,
            MaxBrightness = 0x0F,
            LedsSequencePositions = LedsSequencePositionsConstants.K2,
            LedsCount = LedsSequencePositionsConstants.K2.Length
        };

        public static Keyboard K2 { get; } = new Keyboard()
        {
            Name = "Keychron K2",
            VendorId = 0x05ac,
            ProductId = 0x024f,
            HIDPathPrefix = "\\\\?\\HID#VID_05AC&PID_024F&MI_00",
            MinSpeed = 0x00,
            MaxSpeed = 0x0F,
            MinBrightness = 0x00,
            MaxBrightness = 0x0F,
            LedsSequencePositions = LedsSequencePositionsConstants.K2,
            LedsCount = LedsSequencePositionsConstants.K2.Length
        };
    }
}

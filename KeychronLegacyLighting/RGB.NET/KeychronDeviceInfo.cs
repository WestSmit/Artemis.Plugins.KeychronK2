using RGB.NET.Core;

namespace KeychronLegacyLighting.RGB.NET
{
    public class KeychronDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType => RGBDeviceType.Keyboard;

        public string DeviceName => "Keychron K2";

        public string Manufacturer => "Keychron";

        public string Model => "Keychron K2 RGB";

        public object? LayoutMetadata { get; set; }
    }
}

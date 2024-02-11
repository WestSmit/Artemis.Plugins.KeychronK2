using RGB.NET.Core;

namespace KeychronLegacyLighting.RGB.NET
{
    public class KeychronDevice : AbstractRGBDevice<KeychronDeviceInfo>, IUnknownDevice
    {
        public KeychronDevice(KeychronDeviceInfo deviceInfo, IDeviceUpdateTrigger updateTrigger) : base(deviceInfo, new KeychronUpdateQueue(updateTrigger))
        {
            foreach (var key in LedMappings.K2)
            {
                AddLed(key.ledId, new Point(10, 10), new Size(10, 10));
            }
        }
    }
}

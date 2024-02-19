using RGB.NET.Core;

namespace KeychronLegacyLighting.RGB.NET
{
    public sealed class RGBKeychronDeviceProvider : AbstractRGBDeviceProvider
    {
        private static readonly object _lock = new();
        private static RGBKeychronDeviceProvider? _instance;

        public static RGBKeychronDeviceProvider Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ?? new RGBKeychronDeviceProvider();
                }
            }
        }
        public RGBKeychronDeviceProvider()
        {
            lock (_lock)
            {
                if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(RGBKeychronDeviceProvider)}");
                _instance = this;
            }
        }

        protected override void InitializeSDK() { }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            try
            {
                return RgbController
                    .GetConnectedDevices()
                    .Select(x => new KeychronDevice(new KeychronDeviceInfo(), GetUpdateTrigger()));
            }
            catch (Exception ex)
            {
                Throw(ex);
            }

            return Enumerable.Empty<IRGBDevice>();
        }

        protected override IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit)
        {
            return new DeviceUpdateTrigger(0.1);
        }

        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                base.Dispose(disposing);

                _instance = null;
            }
        }
    }
}

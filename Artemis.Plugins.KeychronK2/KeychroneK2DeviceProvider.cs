using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Core;
using Serilog;
using RGBDeviceProvider = KeychronLegacyLighting.RGB.NET.RGBKeychronDeviceProvider;

namespace Artemis.Plugins.KeychronK2
{
    [PluginFeature(Name = "Keychron K2 Device Provider")]
    public class KeychroneK2DeviceProvider : DeviceProvider
    {
        private readonly ILogger _logger;
        private readonly IDeviceService _deviceService;

        public override RGBDeviceProvider RgbDeviceProvider => RGBDeviceProvider.Instance;

        public KeychroneK2DeviceProvider(ILogger logger, IDeviceService deviceService)
        {
            _logger = logger;
            _deviceService = deviceService;
            CreateMissingLedsSupported = false;
        }

        public override void Disable()
        {

            _deviceService.RemoveDeviceProvider(this);
            RgbDeviceProvider.Exception -= Provider_OnException;
            RgbDeviceProvider.Dispose();
        }

        public override void Enable()
        {
            RgbDeviceProvider.Exception += Provider_OnException;
            _deviceService.AddDeviceProvider(this);
        }

        private void Provider_OnException(object sender, ExceptionEventArgs args) => _logger.Debug(args.Exception, "Keychron Legacy Exception: {message}", args.Exception.Message);
    }
}

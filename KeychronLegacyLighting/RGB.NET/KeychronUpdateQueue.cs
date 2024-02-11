using RGB.NET.Core;

namespace KeychronLegacyLighting.RGB.NET
{
    internal class KeychronUpdateQueue : UpdateQueue
    {
        private readonly RgbController _controller;
        public KeychronUpdateQueue(IDeviceUpdateTrigger updateTrigger) : base(updateTrigger)
        {
            _controller = new RgbController(Keyboard.K2);
            _controller.Connect();
        }

        protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            try
            {
                foreach ((object key, Color color) in dataSet)
                {
                    LedMappings.K2.TryGetValue((LedId)key, out int mapping);

                    _controller.SetColor(mapping, System.Drawing.Color.FromArgb(color.GetR(), color.GetG(), color.GetB()));
                }

                var mode = Mode.GetMode(Modes.CUSTOM);

                _controller.SetMode(mode);

                return true;
            }
            catch (Exception ex)
            {
                RGBKeychronDeviceProvider.Instance.Throw(ex);
            }

            return false;
        }

        public override void Dispose()
        {
            _controller.Dispose();
            base.Dispose();
        }
    }
}

using System.Drawing;

namespace KeychronLegacyLighting
{
    public class Mode
    {
        public string Name { get; private set; }
        public Modes Value { get; private set; }
        public ModeFlags Flags { get; private set; }
        public ColorMode ColorMode { get; set; } = ColorMode.NONE;
        public byte MinBrightness { get; } = Keyboard.Default.MinBrightness;
        public byte MaxBrightness { get; } = Keyboard.Default.MaxBrightness;
        public byte Brightness { get; set; } = Keyboard.Default.MaxBrightness;
        public byte MinSpeed { get; } = Keyboard.Default.MinSpeed;
        public byte MaxSpeed { get; } = Keyboard.Default.MaxSpeed;
        public byte Speed { get; set; } = Keyboard.Default.MinSpeed;
        public byte Direction { get; set; } = 0;
        public Color SpecificColor{ get; set; } = Color.White;

        private Mode() { }

        public static Mode[] DefaultModes { get; } =
        {
            new Mode()
            {
                Name = "Custom",
                Value = Modes.CUSTOM,
                Flags = ModeFlags.HAS_PER_LED_COLOR | ModeFlags.HAS_BRIGHTNESS,
                ColorMode = ColorMode.PER_LED
            },
            new Mode()
            {
                Name = "Static",
                Value = Modes.STATIC,
                Flags =  ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Keystroke light up",
                Value = Modes.KEYSTROKE_LIGHT_UP,
                Flags =  ModeFlags.HAS_PER_LED_COLOR | ModeFlags.HAS_BRIGHTNESS,
                ColorMode = ColorMode.PER_LED
            },
            new Mode()
            {
                Name = "Keystroke dim",
                Value = Modes.KEYSTROKE_DIM,
                Flags =  ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Sparkle",
                Value = Modes.SPARKLE,
                Flags =  ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Rain",
                Value = Modes.RAIN,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Random colors",
                Value = Modes.RANDOM_COLORS,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.RANDOM
            },
            new Mode()
            {
                Name = "Breathing",
                Value = Modes.BREATHING,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Spectrum cycle",
                Value = Modes.SPECTRUM_CYCLE,
                Flags =  ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.RANDOM
            },
            new Mode()
            {
                Name = "Ring gradient",
                Value = Modes.RING_GRADIENT,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED ,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Vertical gradient",
                Value = Modes.VERTICAL_GRADIENT,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED | ModeFlags.HAS_DIRECTION_UD ,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Horizontal gradient / Rainbow wave",
                Value = Modes.HORIZONTAL_GRADIENT_WAVE,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED | ModeFlags.HAS_DIRECTION_LR ,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Around edges",
                Value = Modes.AROUND_EDGES,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Keystroke horizontal lines",
                Value = Modes.KEYSTROKE_HORIZONTAL_LINES_VALUE,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Keystroke tilted lines",
                Value = Modes.KEYSTROKE_TITLED_LINES,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Keystroke ripples",
                Value = Modes.KEYSTROKE_RIPPLES,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Sequence",
                Value = Modes.SEQUENCE,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED | ModeFlags.HAS_DIRECTION_LR,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Wave line",
                Value = Modes.WAVE_LINE,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Tilted lines",
                Value = Modes.TILTED_LINES,
                Flags =  ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Back and forth",
                Value = Modes.BACK_AND_FORTH,
                Flags = ModeFlags.HAS_RANDOM_COLOR | ModeFlags.HAS_MODE_SPECIFIC_COLOR | ModeFlags.HAS_BRIGHTNESS | ModeFlags.HAS_SPEED | ModeFlags.HAS_DIRECTION_LR,
                ColorMode = ColorMode.MODE_SPECIFIC,
            },
            new Mode()
            {
                Name = "Off",
                Value = Modes.LIGHTS_OFF, 
            }
        };

        public static Mode GetMode(Modes mode)
        {
            return DefaultModes.First(x => x.Value == mode);
        }

    }
}

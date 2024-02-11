namespace KeychronLegacyLighting
{
    public enum Commands : byte
    {
        COMMUNICATION_END = 0x02,
        GET_BASIC_INFO = 0x05,
        READ_KEY_DEFINITION_AREA = 0x10,
        WRITE_KEY_DEFINITION_AREA = 0x11,
        READ_LED_EFFECT_DEFINITION_AREA = 0x12,
        WRITE_LED_SPECIAL_EFFECT_AREA = 0x13,
        READ_MACRO_DEFINITION_AREA = 0x14,
        WRITE_MACRO_DEFINITION_AREA = 0x15,
        READ_GAME_MODE_AREA = 0x16,
        WRITE_GAME_MODE_AREA = 0x17,
        TURN_ON_CUSTOMIZATION = 0x18,
        TURN_OFF_CUSTOMIZATION = 0x19,
        LED_EFFECT_START = 0xF0,
        LED_SYNC_INITIAL = 0xF1,
        LED_SYNC_START = 0xF2,
        LED_SYNC_STOP = 0xF3,
        RANDOM_PACKET_START = 0xAB,
    }

    public enum Modes : byte
    {
        CUSTOM = 0x00,
        STATIC = 0x01,
        KEYSTROKE_LIGHT_UP = 0x02,
        KEYSTROKE_DIM = 0x03,
        SPARKLE = 0x04,
        RAIN = 0x05,
        RANDOM_COLORS = 0x06,
        BREATHING = 0x07,
        SPECTRUM_CYCLE = 0x08,
        RING_GRADIENT = 0x09,
        VERTICAL_GRADIENT = 0x0A,
        HORIZONTAL_GRADIENT_WAVE = 0x0B,
        AROUND_EDGES = 0x0C,
        KEYSTROKE_HORIZONTAL_LINES_VALUE = 0x0D,
        KEYSTROKE_TITLED_LINES = 0x0E,
        KEYSTROKE_RIPPLES = 0x0F,
        SEQUENCE = 0x10,
        WAVE_LINE = 0x11,
        TILTED_LINES = 0x12,
        BACK_AND_FORTH = 0x13,
        LIGHTS_OFF = 0x80,
    };

    [Flags]
    public enum ModeFlags : uint
    {
        HAS_SPEED = (1 << 0),                       /* Mode has speed parameter         */
        HAS_DIRECTION_LR = (1 << 1),                /* Mode has left/right parameter    */
        HAS_DIRECTION_UD = (1 << 2),                /* Mode has up/down parameter       */
        HAS_DIRECTION_HV = (1 << 3),                /* Mode has horiz/vert parameter    */
        HAS_BRIGHTNESS = (1 << 4),                  /* Mode has brightness parameter    */
        HAS_PER_LED_COLOR = (1 << 5),               /* Mode has per-LED colors          */
        HAS_MODE_SPECIFIC_COLOR = (1 << 6),         /* Mode has mode specific colors    */
        HAS_RANDOM_COLOR = (1 << 7),                /* Mode has random color option     */
        MANUAL_SAVE = (1 << 8),                     /* Mode can manually be saved       */
        AUTOMATIC_SAVE = (1 << 9),                  /* Mode automatically saves         */
    };

    public enum ColorMode : byte
    {
        NONE = 0,                                   /* Mode has no colors               */
        PER_LED = 1,                                /* Mode has per LED colors selected */
        MODE_SPECIFIC = 2,                          /* Mode specific colors selected    */
        RANDOM = 3,                                 /* Mode has random colors selected  */
    };
}
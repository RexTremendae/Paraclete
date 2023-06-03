namespace Time;

public static class AnsiSequences
{
    // Similar to Clear() but also clears the scrollable buffer, which Clear() doesn't.
    public const string ClearScreen     = "\f\u001bc\x1b[3J";

    public const string Reset           = "\x1b[m";

    public const string Bold            = "\x1b[1m";
    public const string Dim             = "\x1b[2m";
    public const string Italic          = "\x1b[3m";
    public const string SlowBlink       = "\x1b[5m";
    public const string RapidBlink      = "\x1b[6m";
    public const string Reverse         = "\x1b[7m";
    public const string StrikeThrough   = "\x1b[9m";
    public const string Underline       = "\x1b[4m";
    public const string DoubleUnderline = "\x1b[21m";
    public const string NoUnderline     = "\x1b[24m";
    public const string NoBlinking      = "\x1b[25m";
    public const string NoReverse       = "\x1b[27m";
    public const string NoStrikeThrough = "\x1b[29m";

    public static class ForegroundColors
    {
        public static readonly string Black       = ForegroundColor(   0,   0,   0 );
        public static readonly string DarkRed     = ForegroundColor( 100,   0,   0 );
        public static readonly string DarkGreen   = ForegroundColor(   0,  80,   0 );
        public static readonly string DarkYellow  = ForegroundColor(  80,  80,   0 );
        public static readonly string DarkBlue    = ForegroundColor(   0,  50, 120 );
        public static readonly string DarkMagenta = ForegroundColor( 100,   0, 100 );
        public static readonly string DarkCyan    = ForegroundColor(   0, 100, 100 );
        public static readonly string Gray        = ForegroundColor( 100, 100, 100 );
        public static readonly string DarkGray    = ForegroundColor(  50,  50,  50 );
        public static readonly string Red         = ForegroundColor( 200,  50,  50 );
        public static readonly string Green       = ForegroundColor(   0, 200,   0 );
        public static readonly string Yellow      = ForegroundColor( 200, 200,   0 );
        public static readonly string Blue        = ForegroundColor( 100, 150, 255 );
        public static readonly string Magenta     = ForegroundColor( 200,   0, 200 );
        public static readonly string Cyan        = ForegroundColor(   0, 255, 255 );
        public static readonly string White       = ForegroundColor( 255, 255, 255 );
    }

    public static string ForegroundColor(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";
    public static string BackgroundColor(int r, int g, int b) => $"\x1b[48;2;{r};{g};{b}m";
}

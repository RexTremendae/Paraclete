namespace Time;

public static class AnsiSequences
{
    // Similar to Clear() but also clears the scrollable buffer, which Clear() doesn't.
    public static readonly AnsiString ClearScreen     = "\u001bc\u001b[3J";

    public static readonly AnsiString Reset           = "\u001b[m";

    public static readonly AnsiString Bold            = "\u001b[1m";
    public static readonly AnsiString Dim             = "\u001b[2m";
    public static readonly AnsiString Italic          = "\u001b[3m";
    public static readonly AnsiString SlowBlink       = "\u001b[5m";
    public static readonly AnsiString RapidBlink      = "\u001b[6m";
    public static readonly AnsiString Reverse         = "\u001b[7m";
    public static readonly AnsiString StrikeThrough   = "\u001b[9m";
    public static readonly AnsiString Underline       = "\u001b[4m";
    public static readonly AnsiString DoubleUnderline = "\u001b[21m";
    public static readonly AnsiString NoUnderline     = "\u001b[24m";
    public static readonly AnsiString NoBlinking      = "\u001b[25m";
    public static readonly AnsiString NoReverse       = "\u001b[27m";
    public static readonly AnsiString NoStrikeThrough = "\u001b[29m";

    public static class ForegroundColors
    {
        public static readonly AnsiString Black       = ForegroundColor(   0,   0,   0 );
        public static readonly AnsiString DarkRed     = ForegroundColor( 100,   0,   0 );
        public static readonly AnsiString DarkGreen   = ForegroundColor(   0,  80,   0 );
        public static readonly AnsiString DarkYellow  = ForegroundColor(  80,  80,   0 );
        public static readonly AnsiString DarkBlue    = ForegroundColor(   0,  50, 120 );
        public static readonly AnsiString DarkMagenta = ForegroundColor( 100,   0, 100 );
        public static readonly AnsiString DarkCyan    = ForegroundColor(   0, 100, 100 );
        public static readonly AnsiString Gray        = ForegroundColor( 100, 100, 100 );
        public static readonly AnsiString DarkGray    = ForegroundColor(  50,  50,  50 );
        public static readonly AnsiString Red         = ForegroundColor( 200,  50,  50 );
        public static readonly AnsiString Green       = ForegroundColor(   0, 200,   0 );
        public static readonly AnsiString Yellow      = ForegroundColor( 200, 200,   0 );
        public static readonly AnsiString Blue        = ForegroundColor( 100, 150, 255 );
        public static readonly AnsiString Magenta     = ForegroundColor( 200,   0, 200 );
        public static readonly AnsiString Cyan        = ForegroundColor(   0, 255, 255 );
        public static readonly AnsiString White       = ForegroundColor( 255, 255, 255 );
    }

    public static AnsiString ForegroundColor(int r, int g, int b) => $"\u001b[38;2;{r};{g};{b}m";
    public static AnsiString BackgroundColor(int r, int g, int b) => $"\u001b[48;2;{r};{g};{b}m";
}

namespace Paraclete;

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

#pragma warning disable SA1008 // Opening parenthesis should not be followed by a space

    private static readonly (int r, int g, int b) BlackDefinition       = (   0,   0,   0 );
    private static readonly (int r, int g, int b) BlueDefinition        = ( 100, 150, 255 );
    private static readonly (int r, int g, int b) CyanDefinition        = (   0, 255, 255 );
    private static readonly (int r, int g, int b) DarkBlueDefinition    = (   0,  50, 120 );
    private static readonly (int r, int g, int b) DarkCyanDefinition    = (   0, 100, 100 );
    private static readonly (int r, int g, int b) DarkGrayDefinition    = (  50,  50,  50 );
    private static readonly (int r, int g, int b) DarkGreenDefinition   = (   0,  80,   0 );
    private static readonly (int r, int g, int b) DarkMagentaDefinition = ( 100,   0, 100 );
    private static readonly (int r, int g, int b) DarkRedDefinition     = ( 100,   0,   0 );
    private static readonly (int r, int g, int b) DarkYellowDefinition  = (  80,  80,   0 );
    private static readonly (int r, int g, int b) GrayDefinition        = ( 100, 100, 100 );
    private static readonly (int r, int g, int b) GreenDefinition       = (   0, 200,   0 );
    private static readonly (int r, int g, int b) MagentaDefinition     = ( 200,   0, 200 );
    private static readonly (int r, int g, int b) OrangeDefinition      = ( 200, 100,  50 );
    private static readonly (int r, int g, int b) RedDefinition         = ( 200,  50,  50 );
    private static readonly (int r, int g, int b) YellowDefinition      = ( 200, 200,   0 );
    private static readonly (int r, int g, int b) WhiteDefinition       = ( 255, 255, 255 );

#pragma warning restore SA1008 // Opening parenthesis should not be followed by a space

    public static AnsiString ForegroundColor(int r, int g, int b) => $"\u001b[38;2;{r};{g};{b}m";
    public static AnsiString BackgroundColor(int r, int g, int b) => $"\u001b[48;2;{r};{g};{b}m";

    public static class ForegroundColors
    {
        public static readonly AnsiString Black       = ForegroundColor(BlackDefinition.r,       BlackDefinition.g,       BlackDefinition.b      );
        public static readonly AnsiString Blue        = ForegroundColor(BlueDefinition.r,        BlueDefinition.g,        BlueDefinition.b       );
        public static readonly AnsiString Cyan        = ForegroundColor(CyanDefinition.r,        CyanDefinition.g,        CyanDefinition.b       );
        public static readonly AnsiString DarkBlue    = ForegroundColor(DarkBlueDefinition.r,    DarkBlueDefinition.g,    DarkBlueDefinition.b   );
        public static readonly AnsiString DarkCyan    = ForegroundColor(DarkCyanDefinition.r,    DarkCyanDefinition.g,    DarkCyanDefinition.b   );
        public static readonly AnsiString DarkGray    = ForegroundColor(DarkGrayDefinition.r,    DarkGrayDefinition.g,    DarkGrayDefinition.b   );
        public static readonly AnsiString DarkGreen   = ForegroundColor(DarkGreenDefinition.r,   DarkGreenDefinition.g,   DarkGreenDefinition.b  );
        public static readonly AnsiString DarkMagenta = ForegroundColor(DarkMagentaDefinition.r, DarkMagentaDefinition.g, DarkMagentaDefinition.b);
        public static readonly AnsiString DarkRed     = ForegroundColor(DarkRedDefinition.r,     DarkRedDefinition.g,     DarkRedDefinition.b    );
        public static readonly AnsiString DarkYellow  = ForegroundColor(DarkYellowDefinition.r,  DarkYellowDefinition.g,  DarkYellowDefinition.b );
        public static readonly AnsiString Gray        = ForegroundColor(GrayDefinition.r,        GrayDefinition.g,        GrayDefinition.b       );
        public static readonly AnsiString Green       = ForegroundColor(GreenDefinition.r,       GreenDefinition.g,       GreenDefinition.b      );
        public static readonly AnsiString Magenta     = ForegroundColor(MagentaDefinition.r,     MagentaDefinition.g,     MagentaDefinition.b    );
        public static readonly AnsiString Orange      = ForegroundColor(OrangeDefinition.r,      OrangeDefinition.g,      OrangeDefinition.b     );
        public static readonly AnsiString Red         = ForegroundColor(RedDefinition.r,         RedDefinition.g,         RedDefinition.b        );
        public static readonly AnsiString White       = ForegroundColor(WhiteDefinition.r,       WhiteDefinition.g,       WhiteDefinition.b      );
        public static readonly AnsiString Yellow      = ForegroundColor(YellowDefinition.r,      YellowDefinition.g,      YellowDefinition.b     );
    }

    public static class BackgroundColors
    {
        public static readonly AnsiString Black       = BackgroundColor(BlackDefinition.r,       BlackDefinition.g,       BlackDefinition.b      );
        public static readonly AnsiString Blue        = BackgroundColor(BlueDefinition.r,        BlueDefinition.g,        BlueDefinition.b       );
        public static readonly AnsiString Cyan        = BackgroundColor(CyanDefinition.r,        CyanDefinition.g,        CyanDefinition.b       );
        public static readonly AnsiString DarkBlue    = BackgroundColor(DarkBlueDefinition.r,    DarkBlueDefinition.g,    DarkBlueDefinition.b   );
        public static readonly AnsiString DarkCyan    = BackgroundColor(DarkCyanDefinition.r,    DarkCyanDefinition.g,    DarkCyanDefinition.b   );
        public static readonly AnsiString DarkGray    = BackgroundColor(DarkGrayDefinition.r,    DarkGrayDefinition.g,    DarkGrayDefinition.b   );
        public static readonly AnsiString DarkGreen   = BackgroundColor(DarkGreenDefinition.r,   DarkGreenDefinition.g,   DarkGreenDefinition.b  );
        public static readonly AnsiString DarkMagenta = BackgroundColor(DarkMagentaDefinition.r, DarkMagentaDefinition.g, DarkMagentaDefinition.b);
        public static readonly AnsiString DarkRed     = BackgroundColor(DarkRedDefinition.r,     DarkRedDefinition.g,     DarkRedDefinition.b    );
        public static readonly AnsiString DarkYellow  = BackgroundColor(DarkYellowDefinition.r,  DarkYellowDefinition.g,  DarkYellowDefinition.b );
        public static readonly AnsiString Gray        = BackgroundColor(GrayDefinition.r,        GrayDefinition.g,        GrayDefinition.b       );
        public static readonly AnsiString Green       = BackgroundColor(GreenDefinition.r,       GreenDefinition.g,       GreenDefinition.b      );
        public static readonly AnsiString Magenta     = BackgroundColor(MagentaDefinition.r,     MagentaDefinition.g,     MagentaDefinition.b    );
        public static readonly AnsiString Orange      = BackgroundColor(OrangeDefinition.r,      OrangeDefinition.g,      OrangeDefinition.b     );
        public static readonly AnsiString Red         = BackgroundColor(RedDefinition.r,         RedDefinition.g,         RedDefinition.b        );
        public static readonly AnsiString White       = BackgroundColor(WhiteDefinition.r,       WhiteDefinition.g,       WhiteDefinition.b      );
        public static readonly AnsiString Yellow      = BackgroundColor(YellowDefinition.r,      YellowDefinition.g,      YellowDefinition.b     );
    }
}

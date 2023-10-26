namespace Paraclete.Ansi;

public static class AnsiSequences
{
    public const char EscapeCharacter = '\u001b';

    public static readonly AnsiString ClearScreen              = new ($"{EscapeCharacter}[2J");
    public static readonly AnsiString EraseScrollbackBuffer    = new ($"{EscapeCharacter}[3J");

    public static readonly AnsiControlSequence HideCursor      = new ($"{EscapeCharacter}[?25l");
    public static readonly AnsiControlSequence ShowCursor      = new ($"{EscapeCharacter}[?25h");

    public static readonly AnsiControlSequence Reset           = new ($"{EscapeCharacter}[m");

    public static readonly AnsiControlSequence Bold            = new ($"{EscapeCharacter}[1m");
    public static readonly AnsiControlSequence Dim             = new ($"{EscapeCharacter}[2m");
    public static readonly AnsiControlSequence Italic          = new ($"{EscapeCharacter}[3m");
    public static readonly AnsiControlSequence SlowBlink       = new ($"{EscapeCharacter}[5m");
    public static readonly AnsiControlSequence RapidBlink      = new ($"{EscapeCharacter}[6m");
    public static readonly AnsiControlSequence Reverse         = new ($"{EscapeCharacter}[7m");
    public static readonly AnsiControlSequence StrikeThrough   = new ($"{EscapeCharacter}[9m");
    public static readonly AnsiControlSequence Underline       = new ($"{EscapeCharacter}[4m");
    public static readonly AnsiControlSequence DoubleUnderline = new ($"{EscapeCharacter}[21m");
    public static readonly AnsiControlSequence NoUnderline     = new ($"{EscapeCharacter}[24m");
    public static readonly AnsiControlSequence NoBlinking      = new ($"{EscapeCharacter}[25m");
    public static readonly AnsiControlSequence NoReverse       = new ($"{EscapeCharacter}[27m");
    public static readonly AnsiControlSequence NoStrikeThrough = new ($"{EscapeCharacter}[29m");

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

    public static AnsiControlSequence ForegroundColor(int r, int g, int b) => new ($"{EscapeCharacter}[38;2;{r};{g};{b}m");
    public static AnsiControlSequence BackgroundColor(int r, int g, int b) => new ($"{EscapeCharacter}[48;2;{r};{g};{b}m");

    public static class ForegroundColors
    {
        public static readonly AnsiControlSequence Black       = ForegroundColor(BlackDefinition.r,       BlackDefinition.g,       BlackDefinition.b      );
        public static readonly AnsiControlSequence Blue        = ForegroundColor(BlueDefinition.r,        BlueDefinition.g,        BlueDefinition.b       );
        public static readonly AnsiControlSequence Cyan        = ForegroundColor(CyanDefinition.r,        CyanDefinition.g,        CyanDefinition.b       );
        public static readonly AnsiControlSequence DarkBlue    = ForegroundColor(DarkBlueDefinition.r,    DarkBlueDefinition.g,    DarkBlueDefinition.b   );
        public static readonly AnsiControlSequence DarkCyan    = ForegroundColor(DarkCyanDefinition.r,    DarkCyanDefinition.g,    DarkCyanDefinition.b   );
        public static readonly AnsiControlSequence DarkGray    = ForegroundColor(DarkGrayDefinition.r,    DarkGrayDefinition.g,    DarkGrayDefinition.b   );
        public static readonly AnsiControlSequence DarkGreen   = ForegroundColor(DarkGreenDefinition.r,   DarkGreenDefinition.g,   DarkGreenDefinition.b  );
        public static readonly AnsiControlSequence DarkMagenta = ForegroundColor(DarkMagentaDefinition.r, DarkMagentaDefinition.g, DarkMagentaDefinition.b);
        public static readonly AnsiControlSequence DarkRed     = ForegroundColor(DarkRedDefinition.r,     DarkRedDefinition.g,     DarkRedDefinition.b    );
        public static readonly AnsiControlSequence DarkYellow  = ForegroundColor(DarkYellowDefinition.r,  DarkYellowDefinition.g,  DarkYellowDefinition.b );
        public static readonly AnsiControlSequence Gray        = ForegroundColor(GrayDefinition.r,        GrayDefinition.g,        GrayDefinition.b       );
        public static readonly AnsiControlSequence Green       = ForegroundColor(GreenDefinition.r,       GreenDefinition.g,       GreenDefinition.b      );
        public static readonly AnsiControlSequence Magenta     = ForegroundColor(MagentaDefinition.r,     MagentaDefinition.g,     MagentaDefinition.b    );
        public static readonly AnsiControlSequence Orange      = ForegroundColor(OrangeDefinition.r,      OrangeDefinition.g,      OrangeDefinition.b     );
        public static readonly AnsiControlSequence Red         = ForegroundColor(RedDefinition.r,         RedDefinition.g,         RedDefinition.b        );
        public static readonly AnsiControlSequence White       = ForegroundColor(WhiteDefinition.r,       WhiteDefinition.g,       WhiteDefinition.b      );
        public static readonly AnsiControlSequence Yellow      = ForegroundColor(YellowDefinition.r,      YellowDefinition.g,      YellowDefinition.b     );
    }

    public static class BackgroundColors
    {
        public static readonly AnsiControlSequence Black       = BackgroundColor(BlackDefinition.r,       BlackDefinition.g,       BlackDefinition.b      );
        public static readonly AnsiControlSequence Blue        = BackgroundColor(BlueDefinition.r,        BlueDefinition.g,        BlueDefinition.b       );
        public static readonly AnsiControlSequence Cyan        = BackgroundColor(CyanDefinition.r,        CyanDefinition.g,        CyanDefinition.b       );
        public static readonly AnsiControlSequence DarkBlue    = BackgroundColor(DarkBlueDefinition.r,    DarkBlueDefinition.g,    DarkBlueDefinition.b   );
        public static readonly AnsiControlSequence DarkCyan    = BackgroundColor(DarkCyanDefinition.r,    DarkCyanDefinition.g,    DarkCyanDefinition.b   );
        public static readonly AnsiControlSequence DarkGray    = BackgroundColor(DarkGrayDefinition.r,    DarkGrayDefinition.g,    DarkGrayDefinition.b   );
        public static readonly AnsiControlSequence DarkGreen   = BackgroundColor(DarkGreenDefinition.r,   DarkGreenDefinition.g,   DarkGreenDefinition.b  );
        public static readonly AnsiControlSequence DarkMagenta = BackgroundColor(DarkMagentaDefinition.r, DarkMagentaDefinition.g, DarkMagentaDefinition.b);
        public static readonly AnsiControlSequence DarkRed     = BackgroundColor(DarkRedDefinition.r,     DarkRedDefinition.g,     DarkRedDefinition.b    );
        public static readonly AnsiControlSequence DarkYellow  = BackgroundColor(DarkYellowDefinition.r,  DarkYellowDefinition.g,  DarkYellowDefinition.b );
        public static readonly AnsiControlSequence Gray        = BackgroundColor(GrayDefinition.r,        GrayDefinition.g,        GrayDefinition.b       );
        public static readonly AnsiControlSequence Green       = BackgroundColor(GreenDefinition.r,       GreenDefinition.g,       GreenDefinition.b      );
        public static readonly AnsiControlSequence Magenta     = BackgroundColor(MagentaDefinition.r,     MagentaDefinition.g,     MagentaDefinition.b    );
        public static readonly AnsiControlSequence Orange      = BackgroundColor(OrangeDefinition.r,      OrangeDefinition.g,      OrangeDefinition.b     );
        public static readonly AnsiControlSequence Red         = BackgroundColor(RedDefinition.r,         RedDefinition.g,         RedDefinition.b        );
        public static readonly AnsiControlSequence White       = BackgroundColor(WhiteDefinition.r,       WhiteDefinition.g,       WhiteDefinition.b      );
        public static readonly AnsiControlSequence Yellow      = BackgroundColor(YellowDefinition.r,      YellowDefinition.g,      YellowDefinition.b     );
    }
}

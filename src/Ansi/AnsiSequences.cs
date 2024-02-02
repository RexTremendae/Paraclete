namespace Paraclete.Ansi;

public static class AnsiSequences
{
    public const char EscapeCharacter = '\u001b';

    public static readonly AnsiString ClearScreen              = new($"{EscapeCharacter}[2J");
    public static readonly AnsiString EraseScrollbackBuffer    = new($"{EscapeCharacter}[3J");

    public static readonly AnsiControlSequence HideCursor      = new($"{EscapeCharacter}[?25l");
    public static readonly AnsiControlSequence ShowCursor      = new($"{EscapeCharacter}[?25h");

    public static readonly AnsiControlSequence Reset           = new($"{EscapeCharacter}[m");

    public static readonly AnsiControlSequence Bold            = new($"{EscapeCharacter}[1m");
    public static readonly AnsiControlSequence Dim             = new($"{EscapeCharacter}[2m");
    public static readonly AnsiControlSequence Italic          = new($"{EscapeCharacter}[3m");
    public static readonly AnsiControlSequence SlowBlink       = new($"{EscapeCharacter}[5m");
    public static readonly AnsiControlSequence RapidBlink      = new($"{EscapeCharacter}[6m");
    public static readonly AnsiControlSequence Reverse         = new($"{EscapeCharacter}[7m");
    public static readonly AnsiControlSequence StrikeThrough   = new($"{EscapeCharacter}[9m");
    public static readonly AnsiControlSequence Underline       = new($"{EscapeCharacter}[4m");
    public static readonly AnsiControlSequence DoubleUnderline = new($"{EscapeCharacter}[21m");
    public static readonly AnsiControlSequence NoUnderline     = new($"{EscapeCharacter}[24m");
    public static readonly AnsiControlSequence NoBlinking      = new($"{EscapeCharacter}[25m");
    public static readonly AnsiControlSequence NoReverse       = new($"{EscapeCharacter}[27m");
    public static readonly AnsiControlSequence NoStrikeThrough = new($"{EscapeCharacter}[29m");

#pragma warning disable SA1008 // Opening parenthesis should not be followed by a space

    private static readonly (int R, int G, int B) BlackDefinition       = (   0,   0,   0 );
    private static readonly (int R, int G, int B) BlueDefinition        = ( 100, 150, 255 );
    private static readonly (int R, int G, int B) CyanDefinition        = (   0, 255, 255 );
    private static readonly (int R, int G, int B) DarkBlueDefinition    = (   0,  50, 120 );
    private static readonly (int R, int G, int B) DarkCyanDefinition    = (   0, 100, 100 );
    private static readonly (int R, int G, int B) DarkGrayDefinition    = (  50,  50,  50 );
    private static readonly (int R, int G, int B) DarkGreenDefinition   = (   0,  80,   0 );
    private static readonly (int R, int G, int B) DarkMagentaDefinition = ( 100,   0, 100 );
    private static readonly (int R, int G, int B) DarkRedDefinition     = ( 100,   0,   0 );
    private static readonly (int R, int G, int B) DarkYellowDefinition  = (  80,  80,   0 );
    private static readonly (int R, int G, int B) GrayDefinition        = ( 100, 100, 100 );
    private static readonly (int R, int G, int B) GreenDefinition       = (   0, 200,   0 );
    private static readonly (int R, int G, int B) MagentaDefinition     = ( 200,   0, 200 );
    private static readonly (int R, int G, int B) OrangeDefinition      = ( 200, 100,  50 );
    private static readonly (int R, int G, int B) RedDefinition         = ( 200,  50,  50 );
    private static readonly (int R, int G, int B) YellowDefinition      = ( 200, 200,   0 );
    private static readonly (int R, int G, int B) WhiteDefinition       = ( 255, 255, 255 );

#pragma warning restore SA1008 // Opening parenthesis should not be followed by a space

    public static AnsiControlSequence ForegroundColor(int r, int g, int b) => new($"{EscapeCharacter}[38;2;{r};{g};{b}m");
    public static AnsiControlSequence BackgroundColor(int r, int g, int b) => new($"{EscapeCharacter}[48;2;{r};{g};{b}m");

    public static class ForegroundColors
    {
        public static readonly AnsiControlSequence Black       = ForegroundColor(BlackDefinition.R,       BlackDefinition.G,       BlackDefinition.B      );
        public static readonly AnsiControlSequence Blue        = ForegroundColor(BlueDefinition.R,        BlueDefinition.G,        BlueDefinition.B       );
        public static readonly AnsiControlSequence Cyan        = ForegroundColor(CyanDefinition.R,        CyanDefinition.G,        CyanDefinition.B       );
        public static readonly AnsiControlSequence DarkBlue    = ForegroundColor(DarkBlueDefinition.R,    DarkBlueDefinition.G,    DarkBlueDefinition.B   );
        public static readonly AnsiControlSequence DarkCyan    = ForegroundColor(DarkCyanDefinition.R,    DarkCyanDefinition.G,    DarkCyanDefinition.B   );
        public static readonly AnsiControlSequence DarkGray    = ForegroundColor(DarkGrayDefinition.R,    DarkGrayDefinition.G,    DarkGrayDefinition.B   );
        public static readonly AnsiControlSequence DarkGreen   = ForegroundColor(DarkGreenDefinition.R,   DarkGreenDefinition.G,   DarkGreenDefinition.B  );
        public static readonly AnsiControlSequence DarkMagenta = ForegroundColor(DarkMagentaDefinition.R, DarkMagentaDefinition.G, DarkMagentaDefinition.B);
        public static readonly AnsiControlSequence DarkRed     = ForegroundColor(DarkRedDefinition.R,     DarkRedDefinition.G,     DarkRedDefinition.B    );
        public static readonly AnsiControlSequence DarkYellow  = ForegroundColor(DarkYellowDefinition.R,  DarkYellowDefinition.G,  DarkYellowDefinition.B );
        public static readonly AnsiControlSequence Gray        = ForegroundColor(GrayDefinition.R,        GrayDefinition.G,        GrayDefinition.B       );
        public static readonly AnsiControlSequence Green       = ForegroundColor(GreenDefinition.R,       GreenDefinition.G,       GreenDefinition.B      );
        public static readonly AnsiControlSequence Magenta     = ForegroundColor(MagentaDefinition.R,     MagentaDefinition.G,     MagentaDefinition.B    );
        public static readonly AnsiControlSequence Orange      = ForegroundColor(OrangeDefinition.R,      OrangeDefinition.G,      OrangeDefinition.B     );
        public static readonly AnsiControlSequence Red         = ForegroundColor(RedDefinition.R,         RedDefinition.G,         RedDefinition.B        );
        public static readonly AnsiControlSequence White       = ForegroundColor(WhiteDefinition.R,       WhiteDefinition.G,       WhiteDefinition.B      );
        public static readonly AnsiControlSequence Yellow      = ForegroundColor(YellowDefinition.R,      YellowDefinition.G,      YellowDefinition.B     );
    }

    public static class BackgroundColors
    {
        public static readonly AnsiControlSequence Black       = BackgroundColor(BlackDefinition.R,       BlackDefinition.G,       BlackDefinition.B      );
        public static readonly AnsiControlSequence Blue        = BackgroundColor(BlueDefinition.R,        BlueDefinition.G,        BlueDefinition.B       );
        public static readonly AnsiControlSequence Cyan        = BackgroundColor(CyanDefinition.R,        CyanDefinition.G,        CyanDefinition.B       );
        public static readonly AnsiControlSequence DarkBlue    = BackgroundColor(DarkBlueDefinition.R,    DarkBlueDefinition.G,    DarkBlueDefinition.B   );
        public static readonly AnsiControlSequence DarkCyan    = BackgroundColor(DarkCyanDefinition.R,    DarkCyanDefinition.G,    DarkCyanDefinition.B   );
        public static readonly AnsiControlSequence DarkGray    = BackgroundColor(DarkGrayDefinition.R,    DarkGrayDefinition.G,    DarkGrayDefinition.B   );
        public static readonly AnsiControlSequence DarkGreen   = BackgroundColor(DarkGreenDefinition.R,   DarkGreenDefinition.G,   DarkGreenDefinition.B  );
        public static readonly AnsiControlSequence DarkMagenta = BackgroundColor(DarkMagentaDefinition.R, DarkMagentaDefinition.G, DarkMagentaDefinition.B);
        public static readonly AnsiControlSequence DarkRed     = BackgroundColor(DarkRedDefinition.R,     DarkRedDefinition.G,     DarkRedDefinition.B    );
        public static readonly AnsiControlSequence DarkYellow  = BackgroundColor(DarkYellowDefinition.R,  DarkYellowDefinition.G,  DarkYellowDefinition.B );
        public static readonly AnsiControlSequence Gray        = BackgroundColor(GrayDefinition.R,        GrayDefinition.G,        GrayDefinition.B       );
        public static readonly AnsiControlSequence Green       = BackgroundColor(GreenDefinition.R,       GreenDefinition.G,       GreenDefinition.B      );
        public static readonly AnsiControlSequence Magenta     = BackgroundColor(MagentaDefinition.R,     MagentaDefinition.G,     MagentaDefinition.B    );
        public static readonly AnsiControlSequence Orange      = BackgroundColor(OrangeDefinition.R,      OrangeDefinition.G,      OrangeDefinition.B     );
        public static readonly AnsiControlSequence Red         = BackgroundColor(RedDefinition.R,         RedDefinition.G,         RedDefinition.B        );
        public static readonly AnsiControlSequence White       = BackgroundColor(WhiteDefinition.R,       WhiteDefinition.G,       WhiteDefinition.B      );
        public static readonly AnsiControlSequence Yellow      = BackgroundColor(YellowDefinition.R,      YellowDefinition.G,      YellowDefinition.B     );
    }
}

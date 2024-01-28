namespace Paraclete.Configuration;

using Paraclete.Ansi;
using Paraclete.Painting;
using Paraclete.Screens.Chess;

#pragma warning disable CA1822 // Member does not access instance data and can be marked as static

public partial class Settings
{
    public TimeSpan RepaintLoopInterval => TimeSpan.FromMilliseconds(30);
    public bool EnableLogging => false;
}

public partial class Settings
{
    public FpsCounterSettings FpsCounter => new();

    public class FpsCounterSettings
    {
        public bool IsEnabled => false;
        public int HistoryLength => 30;
    }
}

public partial class Settings
{
    public ScreenSaverSettings ScreenSaver => new();

    public class ScreenSaverSettings
    {
        public Font.Size FontSize => Font.Size.L;
        public Font.Size SecondsFontSize => Font.Size.M;
        public TimeSpan ActivationInterval => TimeSpan.FromMinutes(5);
        public TimeSpan ContentChangeInterval => TimeSpan.FromSeconds(30);
    }
}

public partial class Settings
{
    public ColorSettings Colors => new();

    public class ColorSettings
    {
        public AnsiString InputLabel => AnsiSequences.ForegroundColors.White;
        public AnsiString InputData => AnsiSequences.ForegroundColors.Yellow;
        public AnsiString ErroneousInputData => AnsiSequences.ForegroundColors.Red;
    }
}

public partial class Settings
{
    public ChessSettings Chess => new();

    public class ChessSettings
    {
        public ColorSettings Colors => new()
        {
            BlackPlayer = AnsiSequences.ForegroundColors.Magenta,
            WhitePlayer = AnsiSequences.ForegroundColors.White,
            PrimarySelection = AnsiSequences.ForegroundColors.Cyan,
            ShadowPiece = AnsiSequences.ForegroundColors.DarkCyan,
        };

        public BorderStyleDefinition.Style BorderStyle { get; } = BorderStyleDefinition.Style.SingleRoundCorners;
        public bool RotateBoard { get; } = false;

        public class ColorSettings
        {
            public AnsiString BlackPlayer { get; set; } = AnsiSequences.Reset;
            public AnsiString Board { get; set; } = AnsiSequences.Reset;
            public AnsiString CheckIndicator { get; set; } = AnsiSequences.Reset;
            public AnsiString DialogBorder { get; set; } = AnsiSequences.Reset;
            public AnsiString Heading { get; set; } = AnsiSequences.Reset;
            public AnsiString Messages { get; set; } = AnsiSequences.Reset;
            public AnsiString PrimarySelection { get; set; } = AnsiSequences.Reset;
            public AnsiString SecondarySelection { get; set; } = AnsiSequences.Reset;
            public AnsiString ShadowPiece { get; set; } = AnsiSequences.Reset;
            public AnsiString Text { get; set; } = AnsiSequences.Reset;
            public AnsiString WhitePlayer { get; set; } = AnsiSequences.Reset;
        }
    }
}

#pragma warning restore CA1822 // Member does not access instance data and can be marked as static

namespace Paraclete.Configuration;

using Paraclete.Painting;

public partial class Settings
{
    public int RepaintLoopInterval => 30;
    public bool EnableLogging => false;
}

public partial class Settings
{
    public FpsCounterSettings FpsCounter => new ();

    public class FpsCounterSettings
    {
        public bool IsEnabled => false;
        public int HistoryLength => 30;
    }
}

public partial class Settings
{
    public ScreenSaverSettings ScreenSaver => new ();

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
    public ColorSettings Colors => new ();

    public class ColorSettings
    {
        public AnsiString InputLabel => AnsiSequences.ForegroundColors.White;
        public AnsiString InputData => AnsiSequences.ForegroundColors.Yellow;
    }
}

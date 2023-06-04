using Time.Painting;

namespace Time.Configuration;

public class Settings
{
    public int RepaintLoopInterval => 30;

    public class FpsCounterSettings
    {
        public bool IsEnabled => false;
        public int HistoryLength => 30;
    }
    public FpsCounterSettings FpsCounter => new();

    public class ScreenSaverSettings
    {
        public Font.Size FontSize => Font.Size.L;
        public Font.Size SecondsFontSize => Font.Size.M;
        public TimeSpan ActivationInterval => TimeSpan.FromMinutes(5);
        public TimeSpan ContentChangeInterval => TimeSpan.FromSeconds(30);
    }
    public ScreenSaverSettings ScreenSaver => new();
}

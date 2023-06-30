namespace Paraclete.Menu.General;

using Paraclete.Menu.Stopwatch;

public class _MainMenu : MenuBase
{
    public _MainMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand),
    })
    {
    }
}

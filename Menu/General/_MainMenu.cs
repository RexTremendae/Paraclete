using Paraclete.Menu.Stopwatch;

namespace Paraclete.Menu.General;

public class _MainMenu : MenuBase
{
    public _MainMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand)
    })
    {}
}

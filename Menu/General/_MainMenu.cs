using Time.Menu.Stopwatch;

namespace Time.Menu.General;

public class _MainMenu : MenuBase
{
    public _MainMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(ExitApplicationCommand),
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand),
        typeof(GotoTodoMenuCommand)
    })
    {}
}

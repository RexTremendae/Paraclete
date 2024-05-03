namespace Paraclete.Menu.General;

public class MainMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand),
    ])
{
}

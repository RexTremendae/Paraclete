namespace Paraclete.Menu.General;

public class MainMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand),
    })
{
}

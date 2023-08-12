namespace Paraclete.Menu.General;

public class MainMenu : MenuBase
{
    public MainMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartStopCommand),
        typeof(ResetCommand),
        typeof(MarkTimeCommand),
    })
    {
    }
}

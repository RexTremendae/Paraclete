namespace Paraclete.Menu.Git;

public class GitMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(RefreshLogCommand),
    })
{
}

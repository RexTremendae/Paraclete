namespace Paraclete.Menu.Git;

public class GitMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(RefreshLogCommand),
        typeof(ChangeRepositoryCommand),
        typeof(PullCommand),
        typeof(ToggleShowOriginCommand),
        typeof(SelectPreviousCommitCommand),
        typeof(SelectNextCommitCommand),
    })
{
}

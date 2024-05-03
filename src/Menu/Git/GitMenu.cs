namespace Paraclete.Menu.Git;

public class GitMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(RefreshLogCommand),
        typeof(ChangeRepositoryCommand),
        typeof(PullCommand),
        typeof(ToggleShowOriginCommand),
        typeof(SelectPreviousCommitCommand),
        typeof(SelectNextCommitCommand),
    ])
{
}

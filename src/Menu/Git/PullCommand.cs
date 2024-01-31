namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;

public class PullCommand(RepositorySelector repositorySelector, LogStore logStore, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly LogStore _logStore = logStore;
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.P;
    public string Description => "[P]ull";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        await _logStore.Pull(_repositorySelector.SelectedRepository);
        _screenInvalidator.InvalidatePane(1);
    }
}

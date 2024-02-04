namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class RefreshLogCommand(LogStore logStore, RepositorySelector repositorySelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly LogStore _logStore = logStore;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.L;
    public string Description => "Refresh [L]og";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        await _logStore.Refresh(_repositorySelector.SelectedRepository);
        _screenInvalidator.InvalidatePane(GitScreen.Panes.History);
    }
}

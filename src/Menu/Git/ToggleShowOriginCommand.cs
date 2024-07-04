namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class ToggleShowOriginCommand(LogStore logStore, RepositorySelector repositorySelector, ScreenInvalidator screenInvalidator)
    : ToggleCommandBase(ConsoleKey.O, "Show [O]rigin", false)
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly LogStore _logStore = logStore;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public bool IsScreenSaverResistant => true;

    public async override Task Execute()
    {
        Toggle();
        _logStore.SetShowOrigin(State);
        await _logStore.Refresh(_repositorySelector.SelectedRepository);
        _screenInvalidator.InvalidatePane(GitScreen.Panes.History);
    }
}

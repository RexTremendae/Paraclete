namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class ToggleShowOriginCommand(LogStore logStore, RepositorySelector repositorySelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly LogStore _logStore = logStore;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.O;
    public string Description => GetToggledOnText();
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        _showOrigin = !_showOrigin;
        _logStore.SetShowOrigin(_showOrigin);
        await _logStore.Refresh(_repositorySelector.SelectedRepository);
        _screenInvalidator.InvalidatePane(GitScreen.Panes.History);
    }

    private bool _showOrigin;

    private string GetToggledOnText()
    {
        return "Show [O]rigin " + (_showOrigin ? ICommand.FlagChar : ICommand.UnflagChar);
    }
}

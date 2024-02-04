namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class SelectNextCommitCommand(LogStore logStore, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly LogStore _logStore = logStore;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Log line";

    public async Task Execute()
    {
        await _logStore.SelectNextCommit();
        _screenInvalidator.InvalidatePane(GitScreen.Panes.CommitInfo);
    }
}

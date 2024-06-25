namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class ChangeRepositoryCommand(RepositorySelector repositorySelector, ScreenInvalidator screenInvalidator, BusyIndicator busyIndicator)
    : ICommand
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly BusyIndicator _busyIndicator = busyIndicator;

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "Change [R]epository";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        using var busy = _busyIndicator.IndicatePanesAreBusy<GitScreen>(
            "Changing repository...",
            [GitScreen.Panes.History, GitScreen.Panes.CommitInfo]
        );

        await _repositorySelector.SelectNext();
        _screenInvalidator.InvalidatePanes([GitScreen.Panes.Repositories, GitScreen.Panes.History, GitScreen.Panes.CommitInfo]);
    }
}

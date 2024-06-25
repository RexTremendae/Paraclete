namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;
using Paraclete.Screens.Git;

public class PullCommand(RepositorySelector repositorySelector, LogStore logStore, BusyIndicator busyIndicator)
    : ICommand
{
    private readonly LogStore _logStore = logStore;
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly BusyIndicator _busyIndicator = busyIndicator;

    public ConsoleKey Shortcut => ConsoleKey.P;
    public string Description => "[P]ull";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        using var busy = _busyIndicator.IndicatePanesAreBusy<GitScreen>(
            "Waiting for 'git pull'...",
            [GitScreen.Panes.History, GitScreen.Panes.CommitInfo]
        );

        await _logStore.Pull(_repositorySelector.SelectedRepository);
    }
}

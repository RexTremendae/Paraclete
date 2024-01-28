namespace Paraclete.Screens.Git;

using System;
using System.Threading.Tasks;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Git;
using Paraclete.Modules.GitNavigator;
using Paraclete.Painting;

public class GitScreen(
    GitMenu menu,
    RepositorySelector repositorySelector,
    GitRepositorySelectorPainter repositoryListPainter,
    GitLogPainter logPainter)
    : IScreen, IInitializer
{
    private readonly GitRepositorySelectorPainter _repositoryListPainter = repositoryListPainter;
    private readonly GitLogPainter _logPainter = logPainter;
    private readonly RepositorySelector _repositorySelector = repositorySelector;

    private string[] _logLines = [];

    public MenuBase Menu { get; private set; } = menu;

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition(30));

    public string Name => "Git";

    public ConsoleKey Shortcut => ConsoleKey.F5;

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];

        if (paneIndex == 0)
        {
            _repositoryListPainter.PaintRepositoryList(pane, (1, 1));
        }

        if (paneIndex == 1)
        {
            _logPainter.PaintLogLines(pane, (1, 2), _logLines);
        }
    };

    public async Task Initialize(IServiceProvider services)
    {
        var result = await ProcessRunner.ExecuteAsync(
            "git",
            args: ["log", "--oneline", "--pretty=format:%h   %cD   %an   %s", "-20"],
            workingDirectory: _repositorySelector.SelectedRepository,
            launchExternal: false);

        _logLines = result.StdOut;
    }
}

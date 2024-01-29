namespace Paraclete.Screens.Git;

using System;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Git;
using Paraclete.Painting;

public class GitScreen(
    GitMenu menu,
    GitRepositorySelectorPainter repositoryListPainter,
    GitLogPainter logPainter)
    : IScreen
{
    private readonly GitRepositorySelectorPainter _repositoryListPainter = repositoryListPainter;
    private readonly GitLogPainter _logPainter = logPainter;

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
            _logPainter.PaintLogLines(pane, (1, 2));
        }
    };
}

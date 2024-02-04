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

    public ILayout Layout { get; } = new ColumnBasedLayout(
        new ColumnBasedLayout.ColumnDefinition(30),
        new ColumnBasedLayout.ColumnDefinition(0, [0, 20])
    );

    public string Name => "Git";

    public ConsoleKey Shortcut => ConsoleKey.F5;

    public static class Panes
    {
        public const int Repositories = 0;
        public const int History = 1;
        public const int CommitInfo = 2;
    }

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];
        ((Action)(paneIndex switch
        {
            Panes.Repositories => () => _repositoryListPainter.PaintRepositoryList(pane, (1, 1)),
            Panes.History => () => _logPainter.PaintLogLines(pane, (1, 2)),
            Panes.CommitInfo => () => _logPainter.PaintCommitInfo(pane, (1, 1)),
            _ => () => throw new ArgumentOutOfRangeException(message: $"Invalid pane index: {paneIndex}", paramName: nameof(paneIndex)),
        }))();
    };
}

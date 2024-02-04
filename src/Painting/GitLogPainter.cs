namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Modules.GitNavigator;

public class GitLogPainter(Painter painter, LogStore logStore)
{
    private readonly Painter _painter = painter;
    private readonly LogStore _logStore = logStore;

    public void PaintLogLines(Pane pane, (int X, int Y) position)
    {
        var rows =
            0.To(position.Y)
            .Select(_ => AnsiString.Empty)
            .Concat(_logStore.LogLines.Select(_ => _.ToAnsiString()));

        _painter.PaintRows(rows, pane, (position.X, 0), padPaneWidth: true, showEllipsis: true);
    }

    public void PaintCommitInfo(Pane pane, (int X, int Y) position)
    {
        var rows = _logStore.CommitInfoLines.Select(_ => new AnsiString(_));
        _painter.PaintRows(rows, pane, position, padPaneWidth: true, padPaneHeight: true, showEllipsis: true);
    }
}

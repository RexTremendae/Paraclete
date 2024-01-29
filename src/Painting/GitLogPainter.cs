namespace Paraclete.Painting;

using Paraclete.Layouts;
using Paraclete.Modules.GitNavigator;

public class GitLogPainter(Painter painter, LogStore logStore)
{
    private readonly Painter _painter = painter;
    private readonly LogStore _logStore = logStore;

    public void PaintLogLines(Pane pane, (int x, int y) position)
    {
        _painter.PaintRows(_logStore.LogLines.Select(_ => _.ToAnsiString()), pane, position, showEllipsis: true);
    }
}

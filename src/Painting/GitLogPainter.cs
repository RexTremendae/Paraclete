namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

public class GitLogPainter(Painter painter)
{
    private readonly Painter _painter = painter;

    public void PaintLogLines(Pane pane, (int x, int y) position, string[] logLines)
    {
        var rows = new List<AnsiString>();

        rows.AddRange(logLines.Select(_ => new AnsiString(_)));

        _painter.PaintRows(rows, pane, position, showEllipsis: true);
    }
}

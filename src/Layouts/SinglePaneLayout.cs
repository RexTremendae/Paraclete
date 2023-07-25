namespace Paraclete.Layouts;

using Paraclete.Ansi;
using Paraclete.Painting;

public class SinglePaneLayout : ILayout
{
    private Pane _pane = Pane.Empty;
    public Pane[] Panes => new[] { _pane };

    public void Paint(Painter painter)
    {
        var frameRows = new AnsiString[_pane.Size.y + 5];
        var y = 0;

        var emptyRow = string.Empty.PadLeft(_pane.Size.x);
        var singleBarRow = string.Empty.PadLeft(_pane.Size.x, '─');
        var doubleBarRow = string.Empty.PadLeft(_pane.Size.x, '═');

        frameRows[y++] = $"╔{doubleBarRow}╗";

        for (; y < _pane.Size.y + 1;)
        {
            frameRows[y++] = $"║{emptyRow}║";
        }

        frameRows[y++] = $"╟{singleBarRow}╢";
        frameRows[y++] = $"║{emptyRow}║";
        frameRows[y++] = $"║{emptyRow}║";
        frameRows[y++] = $"╚{doubleBarRow}╝";

        painter.PaintRows(frameRows);
    }

    public void Recalculate(int windowWidth, int windowHeight)
    {
        _pane = new Pane((1, 1), (windowWidth - 2, windowHeight - 5), isVisible: true);
    }
}
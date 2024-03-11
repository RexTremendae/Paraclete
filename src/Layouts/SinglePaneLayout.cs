namespace Paraclete.Layouts;

using Paraclete.Ansi;
using Paraclete.Painting;

public class SinglePaneLayout : ILayout
{
    private Pane _pane = Pane.Empty;
    public Pane[] Panes => [_pane];

    public void Paint(Painter painter)
    {
        var frameRows = new AnsiString[_pane.Size.Y + 5];
        var y = 0;

        var emptyRow = string.Empty.PadLeft(_pane.Size.X);
        var singleBarRow = string.Empty.PadLeft(_pane.Size.X, '─');
        var doubleBarRow = string.Empty.PadLeft(_pane.Size.X, '═');

        frameRows[y++] = $"╔{doubleBarRow}╗";

        y.To(_pane.Size.Y + 1).Foreach(_ =>
        {
            frameRows[y++] = $"║{emptyRow}║";
        });

        frameRows[y++] = $"╟{singleBarRow}╢";
        frameRows[y++] = $"║{emptyRow}║";
        frameRows[y++] = $"║{emptyRow}║";
        frameRows[y++] = $"╚{doubleBarRow}╝";

        painter.PaintRows(frameRows);
    }

    public void Recalculate(int windowWidth, int windowHeight)
    {
        _pane = new(
            paneId: 0,
            columnIndex: 0,
            position: (1, 1),
            size: (windowWidth - 2, windowHeight - 5),
            isVisible: true);
    }
}

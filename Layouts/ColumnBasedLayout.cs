using Paraclete.Painting;

namespace Paraclete.Layouts;

public class ColumnBasedLayout : ILayout
{
    private readonly int _1stColumnWidth;

    public ColumnBasedLayout(int firstColumnWidth)
    {
        _1stColumnWidth = firstColumnWidth;
    }

    public void Paint(Painter painter, int windowWidth, int windowHeight)
    {
        var _2ndColumnWidth = windowWidth-3-_1stColumnWidth;

        if (_1stColumnWidth < 0)
            throw new ArgumentOutOfRangeException(nameof(_1stColumnWidth));

        if (_2ndColumnWidth < 0)
            throw new ArgumentOutOfRangeException(nameof(_2ndColumnWidth));

        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(_1stColumnWidth, '═')}╤{"".PadLeft(_2ndColumnWidth, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == 10)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┤{"".PadLeft(_2ndColumnWidth)}║";
            }
            else if (y == windowHeight-4)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┴{"".PadLeft(_2ndColumnWidth, '─')}╢";
            }
            else if (y == windowHeight-2 || y == windowHeight-3)
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(_1stColumnWidth)}│{"".PadLeft(_2ndColumnWidth)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        painter.PaintRows(frameRows);
    }
}

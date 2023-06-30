namespace Paraclete.Layouts;

using Paraclete.Painting;

public class SinglePanelLayout : ILayout
{
    public void Paint(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = $"╔{string.Empty.PadLeft(windowWidth - 2, '═')}╗";

        for (int y = 1; y < windowHeight - 1; y++)
        {
            if (y == windowHeight - 4)
            {
                frameRows[y] = $"╟{string.Empty.PadLeft(windowWidth - 2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{string.Empty.PadLeft(windowWidth - 2)}║";
            }
        }

        frameRows[windowHeight - 1] = $"╚{string.Empty.PadLeft(windowWidth - 2, '═')}╝";

        painter.PaintRows(frameRows);
    }
}

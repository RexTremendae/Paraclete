using Paraclete.Painting;

namespace Paraclete.Layouts;

public class OneFrameLayout : ILayout
{
    public void Paint(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(windowWidth-2, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == windowHeight-4)
            {
                frameRows[y] = $"╟{"".PadLeft(windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        painter.PaintRows(frameRows);
    }
}

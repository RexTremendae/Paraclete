using Time.Menu;
using Time.Menu.Todo;

namespace Time.Screens;

public class ShowroomScreen : ScreenBase
{
    private MenuBase _menu;
    public override MenuBase Menu => _menu;

    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;

    public ShowroomScreen(_ShowroomMenu showroomMenu, Painter painter)
    {
        _menu = showroomMenu;
        _painter = painter;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = 1,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public override void PaintFrame(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new string[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(windowWidth-2, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == windowHeight-3)
            {
                frameRows[y] = $"╟{"".PadLeft(windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        painter.Paint(frameRows);
    }

    public override void PaintContent(Painter painter)
    {
        // Font exhibition
        var text = "0123456789:.";

        var fontWriter = FontWriter.Create(1, '#');
        fontWriter.Write(text, ConsoleColor.Gray, (2, 2), _painter);

        fontWriter = FontWriter.Create(2, '#');
        fontWriter.Write(text, ConsoleColor.DarkCyan, (2, 5), _painter);

        fontWriter = FontWriter.Create(3, '#');
        fontWriter.Write(text, ConsoleColor.DarkBlue, (2, 12), _painter);

        // Current time
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

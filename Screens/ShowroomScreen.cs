using Time.Menu;
using Time.Menu.Todo;

namespace Time.Screens;

public class ShowroomScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;

    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;

    public ShowroomScreen(_ShowroomMenu showroomMenu, Painter painter)
    {
        _menu = showroomMenu;
        _painter = painter;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public void PaintFrame(Painter painter, int windowWidth, int windowHeight)
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

    public void PaintContent(Painter painter)
    {
        // Font exhibition
        var text = "0123456789:.";

        var x = 2;
        var y = 2;

        var colors = new Dictionary<Font.Size, ConsoleColor>{
            { Font.Size.XS, ConsoleColor.DarkBlue },
            { Font.Size.S,  ConsoleColor.Blue },
            { Font.Size.M,  ConsoleColor.DarkCyan },
            { Font.Size.L,  ConsoleColor.Cyan }
        };

        foreach (var size in Enum.GetValues<Font.Size>())
        {
            var fontWriter = FontWriter.Create(size);
            _painter.Paint($"{size.ToString()}:".PadRight(4), (x, y));
            fontWriter.Write(text, colors[size], (x + 4, y), _painter);
            y += fontWriter.Font.CharacterHeight + 1;
        }

        // Current time
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

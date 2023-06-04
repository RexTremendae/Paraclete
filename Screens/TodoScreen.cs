using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ToDoScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;
    public string Name => "ToDo";

    private readonly FrameInvalidator _frameInvalidator;
    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;

    public ToDoScreen(Stopwatch stopWatch, _ToDoMenu toDoMenu, FrameInvalidator frameInvalidator, Painter painter)
    {
        _menu = toDoMenu;
        _frameInvalidator = frameInvalidator;
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

    public void PaintContent(Painter painter)
    {
        painter.PaintRows(
            position: (2, 1),
            rows: new AnsiString[]
            {
                $"{AnsiSequences.ForegroundColors.White}ToDo list:{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.DarkGray}{AnsiSequences.StrikeThrough}Add ToDo section{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Enable add/edit/remove ToDo items{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Persist ToDo items{AnsiSequences.Reset}"
            });

        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

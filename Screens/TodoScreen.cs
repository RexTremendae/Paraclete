using Time.Menu;
using Time.Menu.Todo;

namespace Time.Screens;

public class TodoScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;
    public string Name => "TODO";

    private readonly FrameInvalidator _frameInvalidator;
    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;

    public TodoScreen(Stopwatch stopWatch, _TodoMenu todoMenu, FrameInvalidator frameInvalidator, Painter painter)
    {
        _menu = todoMenu;
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
        // TODOs
        painter.PaintRows(
            position: (2, 1),
            rows: new AnsiString[]
            {
                $"{AnsiSequences.ForegroundColors.White}TODO list:{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.DarkGray}{AnsiSequences.StrikeThrough}Add TODO section{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Enable add/edit/remove TODO items{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Persist TODO items{AnsiSequences.Reset}"
            });

        // Current time
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

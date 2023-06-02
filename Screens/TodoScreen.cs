using Time.Menu;
using Time.Menu.Todo;

namespace Time.Screens;

public class TodoScreen : ScreenBase
{
    private MenuBase _menu;
    public override MenuBase Menu => _menu;

    private readonly FrameInvalidator _frameInvalidator;
    private readonly TimeWriter _currentTimeWriter;
    private readonly Visualizer _visualizer;

    public TodoScreen(Stopwatch stopWatch, _TodoMenu todoMenu, FrameInvalidator frameInvalidator, Visualizer visualizer)
    {
        _menu = todoMenu;
        _frameInvalidator = frameInvalidator;
        _visualizer = visualizer;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = 1,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public override void PaintFrame(Visualizer visualizer, int windowWidth, int windowHeight)
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

        visualizer.Paint(frameRows);
    }

    public override void PaintContent(Visualizer visualizer)
    {
        // TODOs
        visualizer.Paint(
            position: (2, 1),
            rows: new[]
            {
                $"{AnsiSequences.ForegroundColors.White}TODO:{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.DarkGray}{AnsiSequences.StrikeThrough}Add TODO section{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Enable add/edit/remove TODO items{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Persist TODO items{AnsiSequences.Reset}"
            });

        // Current time
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _visualizer);
    }
}

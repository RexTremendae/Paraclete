using static System.Console;

namespace Time;

public class MainLoop
{
    private readonly Visualizer _visualizer;
    private readonly StopWatch _stopWatch;

    public MainLoop(Visualizer visualizer, StopWatch stopWatch)
    {
        _visualizer = visualizer;
        _stopWatch = stopWatch;
    }

    private async Task RepaintLoop()
    {
        var menuOptions = new[] {
            (ConsoleKey.Escape, "Quit"),
            (ConsoleKey.S, "Start/stop"),
            (ConsoleKey.R, "Reset"),
            (ConsoleKey.M, "Mark")
        };

        for(;;)
        {
            _visualizer.PaintFrame(menuOptions);
            _visualizer.PaintContent();
            await Task.Delay(30);
        }
    }

    public void Run()
    {
        Task.Run(async () => await RepaintLoop());

        ConsoleKey? key;

        for(;;)
        {
            key = ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Escape:
                    return;

                case ConsoleKey.S:
                    _stopWatch.ToggleStartStop();
                    break;

                case ConsoleKey.R:
                    _stopWatch.Reset();
                    _visualizer.InvalidateFrame();
                    break;

                case ConsoleKey.M:
                    _stopWatch.Mark();
                    break;

                default:
                    break;
            }
        }
    }
}

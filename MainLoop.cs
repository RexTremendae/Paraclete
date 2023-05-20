using static System.Console;

namespace Time;

public static class MainLoop
{
    public static int updateInterval = 50;
    private static int _windowHeight = 0;
    private static int _windowWidth = 0;

    public static async Task Run()
    {
        var currentTimeSettings = new Settings() with {
            FontSize = 3,
            Color = ConsoleColor.White,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false
        };

        var stopWatchSettings = new Settings() with {
            FontSize = 2,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = ConsoleColor.Magenta,
            MillisecondsColor = ConsoleColor.DarkMagenta
        };

        var now = default(DateTime);
        var currentTimeWriter = new TimeWriter(currentTimeSettings);
        var stopWatchWriter = new TimeWriter(stopWatchSettings);

        var currentTimePosition = (x: 3, y: 2);
        var stopWatchPosition = (x: 3, y: 12);

        ConsoleKey? key;
        var stopWatchStart = default(DateTime);
        var stopWatchStop = default(DateTime);
        var isStopWatchRunning = false;

        var menuOptions = new[] {
            (ConsoleKey.Escape, "Quit"),
            (ConsoleKey.S, "Start/stop"),
            (ConsoleKey.R, "Reset")
        };

        for(;;)
        {
            if (WindowWidth != _windowWidth || WindowHeight != _windowHeight)
            {
                _windowWidth = WindowWidth;
                _windowHeight = WindowHeight;
                PaintFrame(menuOptions);
            }

            key = KeyAvailable ? ReadKey(true).Key : null;

            switch (key)
            {
                case ConsoleKey.Escape:
                    return;

                case ConsoleKey.S:
                    if (isStopWatchRunning)
                    {
                        isStopWatchRunning = false;
                        stopWatchStop = DateTime.Now;
                    }
                    else
                    {
                        isStopWatchRunning = true;
                        if (stopWatchStart == default)
                        {
                            stopWatchStart = DateTime.Now;
                        }
                        else
                        {
                            stopWatchStart += (DateTime.Now - stopWatchStop);
                        }
                    }
                    break;

                case ConsoleKey.R:
                    stopWatchStart = isStopWatchRunning
                        ? DateTime.Now
                        : default;
                    stopWatchStop = default;
                    break;

                default: break;
            }

            now = DateTime.Now;
            currentTimeWriter.Write(now, currentTimePosition);
            var stopWatchTime = (isStopWatchRunning ? DateTime.Now : stopWatchStop)
                - stopWatchStart;
            stopWatchWriter.Write(stopWatchTime, stopWatchPosition);

            await Task.Delay(updateInterval);
        }
    }

    private static void PaintFrame(params (ConsoleKey key, string description)[] menuOptions)
    {
        CursorLeft = 0;
        CursorTop = 0;
        var frameRows = new string[_windowHeight];
        frameRows[0] = $"╔{"".PadLeft(_windowWidth-2, '═')}╗";
        for (int y = 1; y < WindowHeight-1; y++)
        {
            if (y == 10 || y == WindowHeight-3)
            {
                frameRows[y] = $"╟{"".PadLeft(_windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(_windowWidth-2)}║";
            }
        }
        frameRows[_windowHeight-1] = $"╚{"".PadLeft(_windowWidth-2, '═')}╝";

        for (int y = 0; y < _windowHeight; y++)
        {
            CursorLeft = 0;
            CursorTop = y;
            if (_windowHeight != WindowHeight || _windowWidth != WindowWidth)
            {
                return;
            }
            Write(frameRows[y]);
        }

        CursorLeft = 2;
        CursorTop = _windowHeight-2;

        foreach (var (key, description) in menuOptions)
        {
            Write("[");
            ForegroundColor = ConsoleColor.Green;
            Write(key);
            ResetColor();
            Write("] ");
            Write(description + "    ");
        }
    }
}

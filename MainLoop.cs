using static System.Console;

namespace Time;

public static class MainLoop
{
    public static async Task Run()
    {
        var currentTimeSettings = new Settings() with {
            UpdateInterval = 50,
            FontSize = 3,
            DigitDisplayChar = '■',
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false
        };

        var stopWatchSettings = new Settings() with {
            UpdateInterval = 50,
            FontSize = 2,
            DigitDisplayChar = '■',
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = ConsoleColor.Magenta,
            MillisecondsColor = ConsoleColor.DarkMagenta
        };

        var now = default(DateTime);
        var currentTimeWriter = new TimeWriter(currentTimeSettings);
        var stopWatchWriter = new TimeWriter(stopWatchSettings);

        var currentTimePosition = (x: CursorLeft, y: CursorTop);
        var stopWatchPosition = (x: CursorLeft, y: CursorTop + 8);

        ConsoleKey? key;
        var stopWatchStart = default(DateTime);
        var stopWatchStop = default(DateTime);
        var isStopWatchRunning = false;

        for(;;)
        {
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

            await Task.Delay(currentTimeSettings.UpdateInterval);
        }
    }
}

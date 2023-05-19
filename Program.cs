using Time;
using static System.Console;

CursorVisible = false;
Clear();

ConsoleKey? key;
var now = default(DateTime);

var settings = new Settings() with {
    UpdateInterval = 50,
    FontSize = 3,
    DigitDisplayChar = '■',
    ShowHours = true,
    ShowSeconds = true,
    ShowMilliseconds = false
};

var timeWriter = new TimeWriter(settings);

var cursorPos = (x: CursorLeft, y: CursorTop);

for(;;)
{
    key = KeyAvailable ? ReadKey(true).Key : null;

    if (key == ConsoleKey.Escape) break;

    now = DateTime.Now;
    timeWriter.Write(now, cursorPos);

    await Task.Delay(settings.UpdateInterval);
}

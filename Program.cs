using Time;
using static System.Console;

CursorVisible = false;
Clear();

ConsoleKey? key;
var now = default(DateTime);

var timeWriter = new TimeWriter(fontSize: 2);

var cursorPos = (x: CursorLeft, y: CursorTop);

for(;;)
{
    key = KeyAvailable ? ReadKey(true).Key : null;

    if (key == ConsoleKey.Escape) break;

    now = DateTime.Now;
    timeWriter.Write(now, cursorPos);

    await Task.Delay(20);
}

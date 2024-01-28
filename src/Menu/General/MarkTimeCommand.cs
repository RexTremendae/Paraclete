namespace Paraclete.Menu.General;

public class MarkTimeCommand(Stopwatch stopwatch)
    : ICommand
{
    private readonly Stopwatch _stopwatch = stopwatch;

    public ConsoleKey Shortcut => ConsoleKey.M;
    public string Description => "[M]ark";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.Mark();

        return Task.CompletedTask;
    }
}

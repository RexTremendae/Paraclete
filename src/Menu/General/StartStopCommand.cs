namespace Paraclete.Menu.General;

public class StartStopCommand(Stopwatch stopwatch)
    : ICommand
{
    private readonly Stopwatch _stopwatch = stopwatch;

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]tart/stop";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.ToggleStartStop();

        return Task.CompletedTask;
    }
}

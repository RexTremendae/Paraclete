namespace Paraclete.Menu.General;

public class StartStopCommand : ICommand
{
    private readonly Stopwatch _stopwatch;

    public StartStopCommand(Stopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]tart/stop";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.ToggleStartStop();

        return Task.CompletedTask;
    }
}

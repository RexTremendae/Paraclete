namespace Time.Menu.Stopwatch;

public class StartStopCommand : ICommand
{
    private readonly Time.Stopwatch _stopwatch;

    public StartStopCommand(Time.Stopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]tart/stop";

    public Task Execute()
    {
        _stopwatch.ToggleStartStop();

        return Task.CompletedTask;
    }
}

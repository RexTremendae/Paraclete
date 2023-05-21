namespace Time.Menu.Stopwatch;

public class MarkTimeCommand : ICommand
{
    private readonly Time.Stopwatch _stopwatch;

    public MarkTimeCommand(Time.Stopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    public MenuCategory Category => MenuCategory.Stopwatch;
    public ConsoleKey Shortcut => ConsoleKey.M;
    public string Description => "Mark";

    public Task Execute()
    {
        _stopwatch.Mark();

        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.General;

public class ResetCommand(Stopwatch stopwatch, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly Stopwatch _stopwatch = stopwatch;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "[R]eset";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.Reset();
        _screenInvalidator.InvalidateAll();

        return Task.CompletedTask;
    }
}

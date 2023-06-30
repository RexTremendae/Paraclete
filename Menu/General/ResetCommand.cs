namespace Paraclete.Menu.General;

public class ResetCommand : ICommand
{
    private readonly Paraclete.Stopwatch _stopwatch;
    private readonly ScreenInvalidator _screenInvalidator;

    public ResetCommand(Paraclete.Stopwatch stopwatch, ScreenInvalidator screenInvalidator)
    {
        _stopwatch = stopwatch;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "[R]eset";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.Reset();
        _screenInvalidator.Invalidate();

        return Task.CompletedTask;
    }
}

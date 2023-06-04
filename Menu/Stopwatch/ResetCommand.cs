namespace Paraclete.Menu.Stopwatch;

public class ResetCommand : ICommand
{
    private readonly Paraclete.Stopwatch _stopwatch;
    private readonly FrameInvalidator _frameInvalidator;

    public ResetCommand(Paraclete.Stopwatch stopwatch, FrameInvalidator frameInvalidator)
    {
        _stopwatch = stopwatch;
        _frameInvalidator = frameInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "[R]eset";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _stopwatch.Reset();
        _frameInvalidator.Invalidate();

        return Task.CompletedTask;
    }
}

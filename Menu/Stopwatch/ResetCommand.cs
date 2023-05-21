namespace Time.Menu.Stopwatch;

public class ResetCommand : ICommand
{
    private readonly Time.Stopwatch _stopwatch;
    private readonly FrameInvalidator _frameInvalidator;

    public ResetCommand(Time.Stopwatch stopwatch, FrameInvalidator frameInvalidator)
    {
        _stopwatch = stopwatch;
        _frameInvalidator = frameInvalidator;
    }

    public MenuCategory Category => MenuCategory.Stopwatch;
    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "Reset";

    public Task Execute()
    {
        _stopwatch.Reset();
        _frameInvalidator.Invalidate();

        return Task.CompletedTask;
    }
}

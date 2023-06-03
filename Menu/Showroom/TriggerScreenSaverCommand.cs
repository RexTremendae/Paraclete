namespace Time.Menu.Showroom;

public class TriggerScreenSaverCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.S;

    public string Description => "Trigger [S]creen saver";

    public TriggerScreenSaverCommand(ScreenSaver screenSaver)
    {
        _screenSaver = screenSaver;
    }

    private ScreenSaver _screenSaver;

    public Task Execute()
    {
        _screenSaver.Activate();
        return Task.CompletedTask;
    }
}
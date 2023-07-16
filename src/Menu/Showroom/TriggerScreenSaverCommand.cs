namespace Paraclete.Menu.Showroom;

using Paraclete.Screens;

public class TriggerScreenSaverCommand : ICommand
{
    private ScreenSaver _screenSaver;

    public TriggerScreenSaverCommand(ScreenSaver screenSaver)
    {
        _screenSaver = screenSaver;
    }

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "Trigger [S]creen saver";

    public Task Execute()
    {
        _screenSaver.Activate();
        return Task.CompletedTask;
    }
}
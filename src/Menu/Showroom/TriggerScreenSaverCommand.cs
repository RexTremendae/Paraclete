namespace Paraclete.Menu.Showroom;

using Paraclete.Screens;

public class TriggerScreenSaverCommand(ScreenSaver screenSaver)
    : ICommand
{
    private readonly ScreenSaver _screenSaver = screenSaver;

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "Trigger [S]creen saver";

    public Task Execute()
    {
        _screenSaver.Activate();
        return Task.CompletedTask;
    }
}
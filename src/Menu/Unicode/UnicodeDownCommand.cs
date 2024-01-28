namespace Paraclete.Menu.Unicode;

using Paraclete.Screens.Unicode;

public class UnicodeDownCommand(UnicodeControl unicodeControl)
    : ICommand
{
    private readonly UnicodeControl _unicodeControl = unicodeControl;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Scroll down";

    public Task Execute()
    {
        _unicodeControl.ScrollDown();
        return Task.CompletedTask;
    }
}
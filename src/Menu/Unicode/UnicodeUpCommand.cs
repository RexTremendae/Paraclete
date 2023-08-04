namespace Paraclete.Menu.Unicode;

using Paraclete.Screens.Unicode;

public class UnicodeUpCommand : ICommand
{
    private readonly UnicodeControl _unicodeControl;

    public UnicodeUpCommand(UnicodeControl unicodeControl)
    {
        _unicodeControl = unicodeControl;
    }

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Scroll up";

    public Task Execute()
    {
        _unicodeControl.ScrollUp();
        return Task.CompletedTask;
    }
}
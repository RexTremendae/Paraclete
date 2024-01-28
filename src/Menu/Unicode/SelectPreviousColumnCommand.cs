namespace Paraclete.Menu.Unicode;

using Paraclete.Screens.Unicode;

public class SelectPreviousColumnCommand(UnicodeControl unicodeControl)
    : ICommand
{
    private readonly UnicodeControl _unicodeControl = unicodeControl;

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;

    public string Description => "Select previous";

    public Task Execute()
    {
        _unicodeControl.SelectPrevious();
        return Task.CompletedTask;
    }
}

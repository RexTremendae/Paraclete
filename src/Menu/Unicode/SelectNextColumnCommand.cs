namespace Paraclete.Menu.Unicode;

using Paraclete.Screens.Unicode;

public class SelectNextColumnCommand : ICommand
{
    private readonly UnicodeControl _unicodeControl;

    public SelectNextColumnCommand(UnicodeControl unicodeControl)
    {
        _unicodeControl = unicodeControl;
    }

    public ConsoleKey Shortcut => ConsoleKey.RightArrow;

    public string Description => "Next previous";

    public Task Execute()
    {
        _unicodeControl.SelectNext();
        return Task.CompletedTask;
    }
}

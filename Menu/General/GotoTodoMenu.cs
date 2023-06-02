using Time.Screens;

namespace Time.Menu.General;

public class GotoTodoMenuCommand : ICommand
{
    private ScreenSelector _screenSelector;

    public ConsoleKey Shortcut => ConsoleKey.T;

    public string Description => "Edit [T]ODOs";

    public GotoTodoMenuCommand(ScreenSelector screenSelector)
    {
        _screenSelector = screenSelector;
    }

    public Task Execute()
    {
        _screenSelector.SwitchTo<TodoScreen>();
        return Task.CompletedTask;
    }
}

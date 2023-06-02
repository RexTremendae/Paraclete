using Time.Screens;

namespace Time.Menu.General;

public class GotoTodoScreenCommand : ICommand
{
    private ScreenSelector _screenSelector;

    public ConsoleKey Shortcut => ConsoleKey.T;

    public string Description => "Edit [T]ODOs";

    public GotoTodoScreenCommand(ScreenSelector screenSelector)
    {
        _screenSelector = screenSelector;
    }

    public Task Execute()
    {
        _screenSelector.SwitchTo<TodoScreen>();
        return Task.CompletedTask;
    }
}

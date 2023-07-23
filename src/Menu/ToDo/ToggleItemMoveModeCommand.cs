namespace Paraclete.Menu.ToDo;

public class ToggleItemMoveModeCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public ToggleItemMoveModeCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.M;
    public string Description => "Toggle [m]ove mode";

    public Task Execute()
    {
        _toDoList.ToggleMoveItemMode();
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.ToDo;

public class ToggleItemDoneCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public ToggleItemDoneCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.D;
    public string Description => "Toggle [d]one status";

    public async Task Execute()
    {
        await _toDoList.ToggleSelectedDoneState();
        _screenInvalidator.InvalidatePane(0);
    }
}

namespace Paraclete.Menu.ToDo;

public class SortToDoItemCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public SortToDoItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]ort ToDo items";

    public async Task Execute()
    {
        await _toDoList.SortToDoItems();
        _screenInvalidator.InvalidatePane(0);
    }
}

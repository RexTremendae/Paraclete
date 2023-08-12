namespace Paraclete.Menu.ToDo;

public class SortToDoItemsCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public SortToDoItemsCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
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

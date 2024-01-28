namespace Paraclete.Menu.ToDo;

public class SortToDoItemsCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator) : ICommand
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]ort ToDo items";

    public async Task Execute()
    {
        await _toDoList.SortToDoItems();
        _screenInvalidator.InvalidatePane(0);
    }
}

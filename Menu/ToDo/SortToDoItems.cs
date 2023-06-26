namespace Paraclete.Menu.ToDo;

public class SortToDoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.S;
    public string Description => "[S]ort ToDo items";

    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public SortToDoItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public Task Execute()
    {
        _toDoList.SortToDoItems();
        _screenInvalidator.Invalidate();
        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.ToDo;

public class PreviousItemCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public PreviousItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Previous item";

    public async Task Execute()
    {
        await _toDoList.SelectPreviousItem();
        _screenInvalidator.InvalidatePane(0);
    }
}

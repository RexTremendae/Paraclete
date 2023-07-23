namespace Paraclete.Menu.ToDo;

public class NextItemCommand : ICommand
{
    private readonly ToDoList _toDoList;
    private readonly ScreenInvalidator _screenInvalidator;

    public NextItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator, ScreenInvalidator screenInvalidator1)
    {
        _toDoList = toDoList;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Next item";

    public async Task Execute()
    {
        await _toDoList.SelectNextItem();
        _screenInvalidator.InvalidatePane(0);
    }
}

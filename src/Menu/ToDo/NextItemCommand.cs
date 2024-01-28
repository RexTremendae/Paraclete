namespace Paraclete.Menu.ToDo;

public class NextItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Next item";

    public async Task Execute()
    {
        await _toDoList.SelectNextItem();
        _screenInvalidator.InvalidatePane(0);
    }
}

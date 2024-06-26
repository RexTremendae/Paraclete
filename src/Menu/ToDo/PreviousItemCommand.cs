namespace Paraclete.Menu.ToDo;
using Paraclete.Screens;

public class PreviousItemCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator) : ICommand
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Previous item";

    public async Task Execute()
    {
        await _toDoList.SelectPreviousItem();
        _screenInvalidator.InvalidatePane(ToDoScreen.Panes.TodoList);
    }
}

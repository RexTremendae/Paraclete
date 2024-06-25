namespace Paraclete.Menu.ToDo;
using Paraclete.Screens;

public class ToggleItemDoneCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.D;
    public string Description => "Toggle [d]one status";

    public async Task Execute()
    {
        await _toDoList.ToggleSelectedDoneState();
        _screenInvalidator.InvalidatePane(ToDoScreen.Panes.TodoList);
    }
}

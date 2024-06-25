namespace Paraclete.Menu.ToDo;
using Paraclete.Screens;

public class ToggleItemMoveModeCommand(ToDoList toDoList, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.M;
    public string Description => "Toggle [m]ove mode";

    public Task Execute()
    {
        _toDoList.ToggleMoveItemMode();
        _screenInvalidator.InvalidatePane(ToDoScreen.Panes.TodoList);
        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.ToDo;

public class ToggleItemMoveModeCommand : ICommand
{
    private readonly ToDoList _toDoList;

    public ToggleItemMoveModeCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public ConsoleKey Shortcut => ConsoleKey.M;
    public string Description => "Toggle [m]ove mode";

    public Task Execute()
    {
        _toDoList.ToggleMoveItemMode();
        return Task.CompletedTask;
    }
}

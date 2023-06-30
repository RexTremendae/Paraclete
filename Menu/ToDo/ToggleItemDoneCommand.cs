namespace Paraclete.Menu.ToDo;

public class ToggleItemDoneCommand : ICommand
{
    private readonly ToDoList _toDoList;

    public ToggleItemDoneCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public ConsoleKey Shortcut => ConsoleKey.D;
    public string Description => "Toggle [d]one status";

    public async Task Execute()
    {
        await _toDoList.ToggleSelectedDoneState();
    }
}

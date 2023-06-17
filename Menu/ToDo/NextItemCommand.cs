namespace Paraclete.Menu.ToDo;

public class NextItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.DownArrow;

    public string Description => "Next item";

    private readonly ToDoList _toDoList;

    public NextItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public async Task Execute()
    {
        await _toDoList.SelectNextItem();
    }
}

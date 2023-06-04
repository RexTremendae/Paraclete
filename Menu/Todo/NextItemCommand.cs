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

    public Task Execute()
    {
        _toDoList.SelectNextItem();
        return Task.CompletedTask;
    }
}

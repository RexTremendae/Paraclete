namespace Paraclete.Menu.ToDo;

public class PreviousItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.UpArrow;

    public string Description => "Previous item";

    private readonly ToDoList _toDoList;

    public PreviousItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public Task Execute()
    {
        _toDoList.SelectPreviousItem();
        return Task.CompletedTask;
    }
}

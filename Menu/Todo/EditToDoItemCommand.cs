namespace Paraclete.Menu.ToDo;

public class EditToDoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.E;

    public string Description => "[E]dit item";

    private readonly ToDoList _toDoList;

    public EditToDoItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public Task Execute()
    {
        return Task.CompletedTask;
    }
}

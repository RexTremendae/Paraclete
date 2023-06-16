namespace Paraclete.Menu.ToDo;

public class DeleteSelectedToDoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.X;

    public string Description => "Delete selected item";

    private readonly ToDoList _toDoList;

    public DeleteSelectedToDoItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public async Task Execute()
    {
        await _toDoList.DeleteSelectedItem();
    }
}

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

    public async Task Execute()
    {
        await _toDoList.SelectPreviousItem();
    }
}

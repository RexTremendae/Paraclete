namespace Paraclete.Menu.ToDo;

public class PreviousItemCommand : ICommand
{
    private readonly ToDoList _toDoList;

    public PreviousItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Previous item";

    public async Task Execute()
    {
        await _toDoList.SelectPreviousItem();
    }
}

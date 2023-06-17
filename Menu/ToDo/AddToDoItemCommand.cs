namespace Paraclete.Menu.ToDo;

public class AddToDoItemCommand : ICommand, IInputCommand<string>
{
    public ConsoleKey Shortcut => ConsoleKey.A;

    public string Description => "[A]dd item";

    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;

    public AddToDoItemCommand(DataInputter dataInputter, ToDoList toDoList)
    {
        _toDoList = toDoList;
        _dataInputter = dataInputter;
    }

    public Task Execute()
    {
        _dataInputter.StartInput<string>(this, "Enter new ToDo item description:");
        return Task.CompletedTask;
    }

    public async Task CompleteInput(string data)
    {
        await _toDoList.AddItem(new (data));
    }
}

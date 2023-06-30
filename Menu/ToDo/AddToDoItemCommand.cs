namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class AddToDoItemCommand : ICommand, IInputCommand<string>
{
    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;

    public AddToDoItemCommand(DataInputter dataInputter, ToDoList toDoList)
    {
        _toDoList = toDoList;
        _dataInputter = dataInputter;
    }

    public ConsoleKey Shortcut => ConsoleKey.A;
    public string Description => "[A]dd item";

    public async Task Execute()
    {
        await _dataInputter.StartInput(this, "Enter new ToDo item description:");
    }

    public async Task CompleteInput(string data)
    {
        await _toDoList.AddItem(new (data));
    }
}

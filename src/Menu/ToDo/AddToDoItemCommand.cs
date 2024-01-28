namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class AddToDoItemCommand(DataInputter dataInputter, ToDoList toDoList, ScreenInvalidator screenInvalidator)
    : IInputCommand<string>
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.A;
    public string Description => "[A]dd item";

    public async Task Execute()
    {
        await _dataInputter.StartInput(this, "Enter new ToDo item description:");
    }

    public async Task CompleteInput(string data)
    {
        await _toDoList.AddItem(new (data));
        _screenInvalidator.InvalidatePane(0);
    }
}

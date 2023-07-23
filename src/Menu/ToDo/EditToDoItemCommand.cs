namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class EditToDoItemCommand : IInputCommand<string>
{
    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;
    private readonly ScreenInvalidator _screenInvalidator;

    public EditToDoItemCommand(ToDoList toDoList, DataInputter dataInputter, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _dataInputter = dataInputter;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.E;
    public string Description => "[E]dit item";

    public async Task Execute()
    {
        if (_toDoList.SelectedToDoItem == null)
        {
            return;
        }

        var selectedTodoItem = _toDoList.SelectedToDoItem.Description;
        var value = string.IsNullOrEmpty(selectedTodoItem)
            ? null
            : new NullableGeneric<string>(selectedTodoItem);

        await _dataInputter.StartInput(this, "Enter ToDo item description:", value);
    }

    public async Task CompleteInput(string data)
    {
        if (_toDoList.SelectedToDoItem == null)
        {
            throw new InvalidOperationException("No ToDo item selected!");
        }

        _toDoList.SelectedToDoItem.Description = data;
        await _toDoList.Update();
        _screenInvalidator.InvalidatePane(0);
    }
}

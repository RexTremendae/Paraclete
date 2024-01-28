namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class EditToDoItemCommand(ToDoList toDoList, DataInputter dataInputter, ScreenInvalidator screenInvalidator)
    : IInputCommand<string>
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

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
            ? NullableGeneric<string>.CreateNullValue()
            : NullableGeneric<string>.Create(selectedTodoItem);

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

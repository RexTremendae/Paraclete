using Paraclete.IO;

namespace Paraclete.Menu.ToDo;

public class EditToDoItemCommand : IInputCommand<string>
{
    public ConsoleKey Shortcut => ConsoleKey.E;

    public string Description => "[E]dit item";

    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;

    public EditToDoItemCommand(ToDoList toDoList, DataInputter dataInputter)
    {
        _toDoList = toDoList;
        _dataInputter = dataInputter;
    }

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
    }
}

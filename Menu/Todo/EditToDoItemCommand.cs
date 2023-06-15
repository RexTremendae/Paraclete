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

    public Task Execute()
    {
        var selectedTodoItem = _toDoList.SelectedToDoItem.Description;
        var value = string.IsNullOrEmpty(selectedTodoItem)
            ? null
            : new NullableGeneric<string>(selectedTodoItem);

        _dataInputter.StartInput<string>(this, "Enter ToDo item description:", value);
        return Task.CompletedTask;
    }

    public async Task CompleteInput(string data)
    {
        _toDoList.SelectedToDoItem.Description = data;
        await _toDoList.Update();
    }
}

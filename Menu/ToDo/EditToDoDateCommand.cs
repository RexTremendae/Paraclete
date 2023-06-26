using Paraclete.IO;

namespace Paraclete.Menu.ToDo;

public class EditToDoDateCommand : IInputCommand<DateOnly>
{
    public ConsoleKey Shortcut => ConsoleKey.T;

    public string Description => "Edit da[T]e";

    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;

    public EditToDoDateCommand(ToDoList toDoList, DataInputter dataInputter)
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

        var selectedItemExpirationDate = _toDoList.SelectedToDoItem.ExpirationDate;
        await _dataInputter.StartInput(this, "Enter ToDo item expiration date:", selectedItemExpirationDate);
    }

    public async Task CompleteInput(DateOnly data)
    {
        if (_toDoList.SelectedToDoItem == null)
        {
            throw new InvalidOperationException("No ToDo item selected!");
        }

        _toDoList.SelectedToDoItem.ExpirationDate = data;
        await _toDoList.Update();
    }
}

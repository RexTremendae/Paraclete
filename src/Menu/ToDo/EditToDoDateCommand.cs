namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class EditToDoDateCommand(ToDoList toDoList, DataInputter dataInputter, ScreenInvalidator screenInvalidator)
    : IInputCommand<DateOnly>
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.T;
    public string Description => "Edit da[T]e";

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
        _screenInvalidator.InvalidateAll();
    }
}

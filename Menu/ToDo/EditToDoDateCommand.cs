namespace Paraclete.Menu.ToDo;

using Paraclete.IO;

public class EditToDoDateCommand : IInputCommand<DateOnly>
{
    private readonly ToDoList _toDoList;
    private readonly DataInputter _dataInputter;
    private readonly ScreenInvalidator _screenInvalidator;

    public EditToDoDateCommand(ToDoList toDoList, DataInputter dataInputter, ScreenInvalidator screenInvalidator)
    {
        _toDoList = toDoList;
        _dataInputter = dataInputter;
        _screenInvalidator = screenInvalidator;
    }

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
        _screenInvalidator.Invalidate();
    }
}

namespace Paraclete.Menu.ToDo;

using Paraclete.IO;
using Paraclete.Screens;

public class DeleteSelectedToDoItemCommand(ToDoList toDoList, DataInputter dataInputter, Settings settings, ScreenInvalidator screenInvalidator)
    : IInputCommand<bool>
{
    private readonly ToDoList _toDoList = toDoList;
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly Settings _settings = settings;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.X;
    public string Description => "Delete selected item";

    public async Task Execute()
    {
        var text = "Delete '"
            + _settings.Colors.InputData + (_toDoList.SelectedToDoItem?.Description ?? string.Empty)
            + _settings.Colors.InputLabel + "'?";

        await _dataInputter.StartInput(this, text);
    }

    public async Task CompleteInput(bool data)
    {
        if (data)
        {
            await _toDoList.DeleteSelectedItem();
            _screenInvalidator.InvalidatePane(ToDoScreen.Panes.TodoList);
        }
    }
}

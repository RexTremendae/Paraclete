namespace Paraclete;

public class ToDoList : IInitializer
{
    private readonly List<ToDoItem> _toDoItems;
    private readonly List<ToDoItem> _doneItems;
    private readonly ScreenInvalidator _screenInvalidator;

    public IEnumerable<ToDoItem> ToDoItems => _toDoItems;
    public IEnumerable<ToDoItem> DoneItems => _doneItems;

    private List<ToDoItem> _selectedList = new();
    public int _selectedToDoItemIndex;
    public ToDoItem? SelectedToDoItem => _selectedList.Count == 0 ? null : _selectedList[_selectedToDoItemIndex];

    public bool MoveItemMode { get; private set; }
    public int MaxItemLength { get; private set; }

    private const string _todoFilename = "todo.txt";

    public ToDoList(ScreenInvalidator screenInvalidator)
    {
        _screenInvalidator = screenInvalidator;
        _toDoItems = new();
        _doneItems = new();
        _selectedList = _toDoItems;
        UpdateMaxItemLength();
    }

    public void ResetSelection()
    {
        _selectedToDoItemIndex = 0;
        _selectedList = _toDoItems.Any() ? _toDoItems : _doneItems;
        MoveItemMode = false;
    }

    public async Task SelectPreviousItem()
    {
        int newPosition;

        if (_selectedToDoItemIndex == 0)
        {
            if (!MoveItemMode)
            {
                SwitchSelectedList();
            }
            newPosition = _selectedList.Count-1;
        }
        else
        {
            newPosition = _selectedToDoItemIndex-1;
        }

        if (MoveItemMode)
        {
            var itemToMove = _selectedList[_selectedToDoItemIndex];
            _selectedList.RemoveAt(_selectedToDoItemIndex);
            _selectedList.Insert(newPosition, itemToMove);
        }
        _selectedToDoItemIndex = newPosition;
        await Update();
    }

    public async Task SelectNextItem()
    {
        int newPosition;

        if (_selectedToDoItemIndex >= _selectedList.Count-1)
        {
            if (!MoveItemMode)
            {
                SwitchSelectedList();
            }
            newPosition = 0;
        }
        else
        {
            newPosition = _selectedToDoItemIndex+1;
        }

        if (MoveItemMode)
        {
            var itemToMove = _selectedList[_selectedToDoItemIndex];
            _selectedList.RemoveAt(_selectedToDoItemIndex);
            _selectedList.Insert(newPosition, itemToMove);
        }
        _selectedToDoItemIndex = newPosition;
        await Update();
    }

    private void SwitchSelectedList()
    {
        var newSelectedList = (_selectedList == _toDoItems ? _doneItems : _toDoItems);
        if (newSelectedList.Count > 0)
        {
            _selectedList = (_selectedList == _toDoItems ? _doneItems : _toDoItems);
        }
    }

    public async Task ToggleSelectedDoneState()
    {
        var todoItem = _selectedList[_selectedToDoItemIndex];
        _selectedList.Remove(todoItem);
        (_selectedList == _toDoItems ? _doneItems : _toDoItems).Add(todoItem);

        if (!_selectedList.Any())
        {
            SwitchSelectedList();
        }
        else if (_selectedToDoItemIndex >= _selectedList.Count)
        {
            _selectedToDoItemIndex = _selectedList.Count - 1;
        }

        await Update();
    }

    public void ToggleMoveItemMode()
    {
        MoveItemMode = !MoveItemMode;
    }

    public async Task AddItem(string description)
    {
        _toDoItems.Add(new(description));
        await Update();
    }

    public async Task DeleteSelectedItem()
    {
        _toDoItems.RemoveAt(_selectedToDoItemIndex);
        if (_selectedToDoItemIndex >= _toDoItems.Count)
        {
            _selectedToDoItemIndex = Math.Max(_toDoItems.Count-1, 0);
        }

        await Update();
        _screenInvalidator.Invalidate();
    }

    public void UpdateMaxItemLength()
    {
        var maxToDoLength = _toDoItems.Any()
            ? _toDoItems.Max(_ => _.ToDisplayString(false).Length)
            : 0;

        var maxDoneLength = _doneItems.Any()
            ? _doneItems.Max(_ => _.ToDisplayString(true).Length)
            : 0;

        MaxItemLength = int.Max(maxToDoLength, maxDoneLength);
    }

    public async Task Initialize()
    {
        if (!File.Exists(_todoFilename))
        {
            return;
        }

        foreach (var line in await File.ReadAllLinesAsync(_todoFilename))
        {
            if (string.IsNullOrEmpty(line)) continue;
            var done = line[0] == '-';

            (done ? _doneItems : _toDoItems).Add(new(
                description: line[13..],
                expirationDate: DateOnly.Parse(line[2..12])));
        }

        UpdateMaxItemLength();
    }

    public async Task Update()
    {
        UpdateMaxItemLength();
        await File.WriteAllLinesAsync(_todoFilename,
            _toDoItems.Select(_ => _.ToPersistString(false))
            .Concat(_doneItems.Select(_ => _.ToPersistString(true)))
        );
    }
}

public class ToDoItem
{
    public string Description { get; set; }
    public DateOnly ExpirationDate { get; }

    public ToDoItem(string description, DateOnly expirationDate = default)
    {
        Description = description;
        ExpirationDate = expirationDate;
    }

    public AnsiString ToDisplayString(bool done)
    {
        var now = DateOnly.FromDateTime(DateTime.Now.Date);
        var descriptionColor = done
            ? AnsiSequences.ForegroundColors.Gray
            : now switch
            {
                var x when ExpirationDate == default => AnsiSequences.ForegroundColors.Yellow,
                var x when ExpirationDate < now      => AnsiSequences.ForegroundColors.Red,
                var x when ExpirationDate == now     => AnsiSequences.ForegroundColors.Orange,
                _                                    => AnsiSequences.ForegroundColors.Yellow
            };

        var expirationDate = (ExpirationDate != default
            ? AnsiSequences.ForegroundColors.Gray + ExpirationDate.ToString(" (yyyy-MM-dd)")
            : "");

        return descriptionColor + Description + expirationDate + AnsiSequences.Reset;
    }

    public string ToPersistString(bool done) =>
        (done ? "- " : "  ") +
        ExpirationDate.ToString("yyyy-MM-dd ") +
        Description;
}

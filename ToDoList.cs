namespace Paraclete;

public class ToDoList : IInitializer
{
    private List<ToDoItem> _toDoItems;
    public IEnumerable<ToDoItem> ToDoItems => _toDoItems;

    public int _selectedToDoItemIndex;
    public ToDoItem SelectedToDoItem => _toDoItems[_selectedToDoItemIndex];

    public bool MoveItemMode { get; private set; }
    public int MaxItemLength { get; private set; }

    const string _todoFileName = "todo.txt";

    public ToDoList()
    {
        _toDoItems = new();
        UpdateMaxItemLength();
    }

    public void ResetSelection()
    {
        _selectedToDoItemIndex = 0;
        MoveItemMode = false;
    }

    public async Task SelectPreviousItem()
    {
        int newPosition;

        if (_selectedToDoItemIndex == 0)
        {
            newPosition = _toDoItems.Count-1;
        }
        else
        {
            newPosition = _selectedToDoItemIndex-1;
        }

        if (MoveItemMode)
        {
            var itemToMove = _toDoItems[_selectedToDoItemIndex];
            _toDoItems.RemoveAt(_selectedToDoItemIndex);
            _toDoItems.Insert(newPosition, itemToMove);
        }
        _selectedToDoItemIndex = newPosition;
        await Update();
    }

    public async Task SelectNextItem()
    {
        int newPosition;

        if (_selectedToDoItemIndex >= _toDoItems.Count-1)
        {
            newPosition = 0;
        }
        else
        {
            newPosition = _selectedToDoItemIndex+1;
        }

        if (MoveItemMode)
        {
            var itemToMove = _toDoItems[_selectedToDoItemIndex];
            _toDoItems.RemoveAt(_selectedToDoItemIndex);
            _toDoItems.Insert(newPosition, itemToMove);
        }
        _selectedToDoItemIndex = newPosition;
        await Update();
    }

    public async Task ToggleSelectedDoneState()
    {
        SelectedToDoItem.ToggleDoneState();
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

    public void UpdateMaxItemLength()
    {
        MaxItemLength = _toDoItems.Any()
            ? _toDoItems.Max(_ => _.Description.Length)
            : 0;
    }

    public async Task Initialize()
    {
        if (!File.Exists(_todoFileName))
        {
            return;
        }

        foreach (var line in await File.ReadAllLinesAsync(_todoFileName))
        {
            if (string.IsNullOrEmpty(line)) continue;
            _toDoItems.Add(new(line[1..], done: line[0] == '-'));
        }

        UpdateMaxItemLength();
    }

    public async Task Update()
    {
        UpdateMaxItemLength();
        await File.WriteAllLinesAsync(_todoFileName, _toDoItems.Select(_ => (_.Done ? '-' : ' ') + _.Description));
    }
}

public class ToDoItem
{
    public string Description { get; set; }
    public bool Done { get; private set; }

    public ToDoItem(string description, bool done = false)
    {
        Description = description;
        Done = done;
    }

    public void ToggleDoneState()
    {
        Done = !Done;
    }
}

namespace Paraclete;

using System.Globalization;

public class ToDoList : IInitializer
{
    private const string _todoFilename = "todo.txt";

    private readonly List<ToDoItem> _toDoItems;
    private readonly List<ToDoItem> _doneItems;

    private int _selectedToDoItemIndex;
    private List<ToDoItem> _selectedList;

    public ToDoList()
    {
        _toDoItems = [];
        _doneItems = [];
        _selectedList = _toDoItems;
        UpdateMaxItemLength();
    }

    public ToDoItem? SelectedToDoItem => _selectedList.Count == 0 ? null : _selectedList[_selectedToDoItemIndex];

    public IEnumerable<ToDoItem> ToDoItems => _toDoItems;
    public IEnumerable<ToDoItem> DoneItems => _doneItems;

    public bool MoveItemMode { get; private set; }
    public int MaxItemLength { get; private set; }

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

            newPosition = _selectedList.Count - 1;
        }
        else
        {
            newPosition = _selectedToDoItemIndex - 1;
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

        if (_selectedToDoItemIndex >= _selectedList.Count - 1)
        {
            if (!MoveItemMode)
            {
                SwitchSelectedList();
            }

            newPosition = 0;
        }
        else
        {
            newPosition = _selectedToDoItemIndex + 1;
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

    public async Task ToggleSelectedDoneState()
    {
        if (!_selectedList.Any())
        {
            return;
        }

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
        if (!_selectedList.Any())
        {
            SwitchSelectedList();
        }

        await Update();
    }

    public async Task DeleteSelectedItem()
    {
        if (!_selectedList.Any())
        {
            return;
        }

        _selectedList.RemoveAt(_selectedToDoItemIndex);
        if (_selectedToDoItemIndex >= _selectedList.Count)
        {
            _selectedToDoItemIndex = Math.Max(_selectedList.Count - 1, 0);
        }

        await Update();
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

    public async Task Initialize(IServiceProvider services)
    {
        if (!File.Exists(_todoFilename))
        {
            return;
        }

        foreach (var line in await File.ReadAllLinesAsync(_todoFilename))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var done = line[0] == '-';

            (done ? _doneItems : _toDoItems).Add(new(
                description: line[13..],
                expirationDate: DateOnly.Parse(line[2..12], CultureInfo.InvariantCulture)));
        }

        UpdateMaxItemLength();
    }

    public async Task SortToDoItems()
    {
        _toDoItems.Sort((first, second) => 0 switch
        {
            var _ when first.ExpirationDate == default && second.ExpirationDate != default => 1,
            var _ when first.ExpirationDate != default && second.ExpirationDate == default => -1,
            var _ when first.ExpirationDate > second.ExpirationDate => 1,
            var _ when first.ExpirationDate < second.ExpirationDate => -1,
            _ => 0,
        });

        await Update();
    }

    public async Task Update()
    {
        UpdateMaxItemLength();
        await File.WriteAllLinesAsync(
            _todoFilename,
            _toDoItems.Select(_ => _.ToPersistString(false))
            .Concat(_doneItems.Select(_ => _.ToPersistString(true)))
        );
    }

    private void SwitchSelectedList()
    {
        var newSelectedList = (_selectedList == _toDoItems ? _doneItems : _toDoItems);
        if (newSelectedList.Count > 0)
        {
            _selectedList = (_selectedList == _toDoItems ? _doneItems : _toDoItems);
        }
    }
}

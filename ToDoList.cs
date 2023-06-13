namespace Paraclete;

public class ToDoList
{
    private List<ToDoItem> _toDoItems;
    public IEnumerable<ToDoItem> ToDoItems => _toDoItems;

    public int _selectedToDoItemIndex;
    public ToDoItem SelectedToDoItem => _toDoItems[_selectedToDoItemIndex];

    public bool MoveItemMode { get; private set; }
    public int MaxItemLength { get; private set; }

    public ToDoList()
    {
        _toDoItems = new()
        {
            new ToDoItem("Add ToDo section", true),
            new ToDoItem("Enable add/edit/remove ToDo items"),
            new ToDoItem("Persist ToDo items")
        };
        UpdateMaxItemLength();
    }

    public void ResetSelection()
    {
        _selectedToDoItemIndex = 0;
        MoveItemMode = false;
    }

    public void SelectPreviousItem()
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
    }

    public void SelectNextItem()
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
    }

    public void ToggleSelectedDoneState()
    {
        SelectedToDoItem.ToggleDoneState();
    }

    public void ToggleMoveItemMode()
    {
        MoveItemMode = !MoveItemMode;
    }

    public void AddItem(string description)
    {
        _toDoItems.Add(new(description));
        UpdateMaxItemLength();
    }

    public void UpdateMaxItemLength()
    {
        MaxItemLength = _toDoItems.Max(_ => _.Description.Length);
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

namespace Paraclete;

public class ToDoList
{
    private List<ToDoItem> _toDoItems;
    public IEnumerable<ToDoItem> ToDoItems => _toDoItems;

    public ToDoList()
    {
        _toDoItems = new()
        {
            new ToDoItem("Add ToDo section", true),
            new ToDoItem("Enable add/edit/remove ToDo items"),
            new ToDoItem("Persist ToDo items")
        };
    }
}

public class ToDoItem
{
    public string Description { get; }
    public bool Done { get; }

    public ToDoItem(string description, bool done = false)
    {
        Description = description;
        Done = done;
    }
}

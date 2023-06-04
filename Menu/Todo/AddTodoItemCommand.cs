namespace Paraclete.Menu.ToDo;

public class AddToDoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.A;

    public string Description => "[A]dd item";

    private readonly ToDoList _toDoList;

    public AddToDoItemCommand(ToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public Task Execute()
    {
        var newItems = new[]
        {
            "Clean the kitchen",
            "Go shopping",
            "Sign up for a knitting class",
            "Rob a bank",
            "Learn Mandarin",
            "Punch yourself in the face",
            "Start smoking",
            "Buy a car",
            "Watch TV",
            "Compile a Linux kernel"
        };

        _toDoList.AddItem(newItems[Random.Shared.Next(newItems.Length)]);
        return Task.CompletedTask;
    }
}

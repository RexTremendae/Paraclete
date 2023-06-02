namespace Time.Menu.Todo;

public class AddTodoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.A;

    public string Description => "Add item";

    public Task Execute()
    {
        throw new NotImplementedException();
    }
}

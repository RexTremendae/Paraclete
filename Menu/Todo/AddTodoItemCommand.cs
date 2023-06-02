namespace Time.Menu.Todo;

public class AddTodoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.A;

    public string Description => "[A]dd item";

    public Task Execute()
    {
        throw new NotImplementedException();
    }
}

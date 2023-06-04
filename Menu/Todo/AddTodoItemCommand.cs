namespace Paraclete.Menu.ToDo;

public class AddToDoItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.A;

    public string Description => "[A]dd item";

    public Task Execute()
    {
        throw new NotImplementedException();
    }
}

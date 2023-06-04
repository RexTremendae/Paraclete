namespace Paraclete.Menu.ToDo;

public class SelectPreviousItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.UpArrow;

    public string Description => "Select previous item";

    public Task Execute()
    {
        return Task.CompletedTask;
    }
}

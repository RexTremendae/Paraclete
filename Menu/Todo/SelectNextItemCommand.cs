namespace Paraclete.Menu.ToDo;

public class SelectNextItemCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.DownArrow;

    public string Description => "Select next item";

    public Task Execute()
    {
        return Task.CompletedTask;
    }
}

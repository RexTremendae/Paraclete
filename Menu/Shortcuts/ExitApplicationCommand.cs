namespace Paraclete.Menu.Shortcuts;

public class ExitApplicationCommand : StartProcessCommandBase, IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.Q;
    public string Description => "[Q]uit";
    public string LongDescription => $"Quit {nameof(Paraclete)}";

    public Task Execute()
    {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}

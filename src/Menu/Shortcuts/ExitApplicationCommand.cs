namespace Paraclete.Menu.Shortcuts;

public class ExitApplicationCommand : IShortcut
{
    private readonly Terminator _terminator;

    public ExitApplicationCommand(Terminator terminator)
    {
        _terminator = terminator;
    }

    public ConsoleKey Shortcut => ConsoleKey.Q;
    public string Description => "[Q]uit";
    public string LongDescription => $"Quit {nameof(Paraclete)}";

    public Task Execute()
    {
        _terminator.RequestTermination();
        return Task.CompletedTask;
    }
}

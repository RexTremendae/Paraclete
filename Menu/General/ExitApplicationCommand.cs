namespace Paraclete.Menu.General;

public class ExitApplicationCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.Escape;
    public string Description => "Quit";

    public Task Execute()
    {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}

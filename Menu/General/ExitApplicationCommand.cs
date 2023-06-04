namespace Paraclete.Menu.General;

public class ExitApplicationCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.Escape;
    public string Description => "Quit";

    public async Task Execute()
    {
        await Task.CompletedTask;
    }
}

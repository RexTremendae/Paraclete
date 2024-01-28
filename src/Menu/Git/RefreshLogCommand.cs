namespace Paraclete.Menu.Git;

public class RefreshLogCommand()
    : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.L;
    public string Description => "Refresh [L]og";
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        return Task.CompletedTask;
    }
}

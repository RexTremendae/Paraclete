namespace Time.Menu.General;

public class ExitApplicationCommand : ICommand
{
    public MenuCategory Category => MenuCategory.General;
    public ConsoleKey Shortcut => ConsoleKey.Escape;
    public string Description => "Quit";

    public async Task Execute()
    {
        await Task.CompletedTask;
    }
}

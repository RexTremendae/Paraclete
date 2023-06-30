namespace Paraclete.Menu;

using Paraclete.Screens;

public class GotoMainMenuCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.Escape;

    public string Description => "Return Home";

    private readonly ScreenSelector _screenSelector;

    public GotoMainMenuCommand(ScreenSelector screenSelector)
    {
        _screenSelector = screenSelector;
    }

    public Task Execute()
    {
        _screenSelector.SwitchTo<HomeScreen>();
        return Task.CompletedTask;
    }
}

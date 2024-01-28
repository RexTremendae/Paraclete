namespace Paraclete.Menu;

using Paraclete.Screens;

public class GotoMainMenuCommand(ScreenSelector screenSelector)
    : ICommand
{
    private readonly ScreenSelector _screenSelector = screenSelector;

    public ConsoleKey Shortcut => ConsoleKey.Escape;

    public string Description => "Return Home";

    public Task Execute()
    {
        _screenSelector.SwitchTo<HomeScreen>();
        return Task.CompletedTask;
    }
}

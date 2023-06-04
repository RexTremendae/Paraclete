using Paraclete.Screens;

namespace Paraclete.Menu;

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

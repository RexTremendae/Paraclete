namespace Paraclete.Menu;

using Paraclete.Screens;

public class SwitchScreenCommand(ConsoleKey shortcut, IScreen screen, ScreenSelector screenSelector)
    : ICommand
{
    private readonly ScreenSelector _screenSelector = screenSelector;
    private readonly IScreen _screen = screen;

    public ConsoleKey Shortcut { get; } = shortcut;
    public string Description { get; } = "Switch to " + screen.Name;
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _screenSelector.SwitchTo(_screen);
        return Task.CompletedTask;
    }
}

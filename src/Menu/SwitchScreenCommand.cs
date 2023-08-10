namespace Paraclete.Menu;

using Paraclete.Screens;

public class SwitchScreenCommand : ICommand
{
    private readonly ScreenSelector _screenSelector;
    private readonly IScreen _screen;

    public SwitchScreenCommand(ConsoleKey shortcut, IScreen screen, ScreenSelector screenSelector)
    {
        _screenSelector = screenSelector;
        _screen = screen;
        Shortcut = shortcut;
        Description = "Switch to " + screen.Name;
    }

    public ConsoleKey Shortcut { get; }
    public string Description { get; }
    public bool IsScreenSaverResistant => true;

    public Task Execute()
    {
        _screenSelector.SwitchTo(_screen);
        return Task.CompletedTask;
    }
}

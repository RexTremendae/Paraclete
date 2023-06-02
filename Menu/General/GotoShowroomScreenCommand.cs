using Time.Screens;

namespace Time.Menu.General;

public class GotoShowroomScreenCommand : ICommand
{
    private ScreenSelector _screenSelector;

    public ConsoleKey Shortcut => ConsoleKey.W;

    public string Description => "Sho[W]room";

    public GotoShowroomScreenCommand(ScreenSelector screenSelector)
    {
        _screenSelector = screenSelector;
    }

    public Task Execute()
    {
        _screenSelector.SwitchTo<ShowroomScreen>();
        return Task.CompletedTask;
    }
}

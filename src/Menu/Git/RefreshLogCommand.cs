namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;

public class RefreshLogCommand(LogStore logStore, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly LogStore _logStore = logStore;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.L;
    public string Description => "Refresh [L]og";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        await _logStore.Refresh();
        _screenInvalidator.InvalidatePane(1);
    }
}

namespace Paraclete.Menu.Git;

using Paraclete.Modules.GitNavigator;

public class ChangeRepositoryCommand(RepositorySelector repositorySelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => "Change [R]epository";
    public bool IsScreenSaverResistant => true;

    public async Task Execute()
    {
        await _repositorySelector.SelectNext();
        _screenInvalidator.InvalidateAll();
    }
}

namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class RotateBoardCommand(ScreenInvalidator screenInvalidator, Settings settings)
    : ICommand
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly Settings.ChessSettings _settings = settings.Chess;

    public ConsoleKey Shortcut => ConsoleKey.R;
    public string Description => GetToggledOnText();

    public Task Execute()
    {
        _settings.RotateBoard = !_settings.RotateBoard;
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }

    private string GetToggledOnText()
    {
        return "[R]otate board " + (_settings.RotateBoard ? ICommand.FlagChar : ICommand.UnflagChar);
    }
}

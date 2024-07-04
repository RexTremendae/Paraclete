namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class RotateBoardCommand(ScreenInvalidator screenInvalidator, Settings settings)
    : ToggleCommandBase(ConsoleKey.R, "[R]otate board", settings.Chess.RotateBoard)
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly Settings.ChessSettings _settings = settings.Chess;

    public override Task Execute()
    {
        Toggle();
        _settings.RotateBoard = State;
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

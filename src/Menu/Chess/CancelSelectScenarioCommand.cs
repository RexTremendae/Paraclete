namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class CancelSelectScenarioCommand : ICommand
{
    private readonly ChessScreen _chessScreen;
    private readonly ScreenInvalidator _screenInvalidator;

    public CancelSelectScenarioCommand(ChessScreen chessScreen, ScreenInvalidator screenInvalidator)
    {
        _chessScreen = chessScreen;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.Escape;

    public string Description => "[] Cancel";

    public Task Execute()
    {
        _chessScreen.CancelSelectScenario();
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}

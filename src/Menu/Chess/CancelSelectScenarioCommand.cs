namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class CancelSelectScenarioCommand(ChessScreen chessScreen, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ChessScreen _chessScreen = chessScreen;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.Escape;

    public string Description => "[] Cancel";

    public Task Execute()
    {
        _chessScreen.CancelSelectScenario();
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class ListScenariosCommand(ChessScreen chessScreen, ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ChessScreen _chessScreen = chessScreen;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly ScenarioSelector _scenarioSelector = scenarioSelector;

    public ConsoleKey Shortcut => ConsoleKey.S;

    public string Description => "Select [S]cenario";

    public Task Execute()
    {
        _chessScreen.StartSelectScenario();
        _scenarioSelector.StartSelectScenario();
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}

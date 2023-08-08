namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class ListScenariosCommand : ICommand
{
    private readonly ChessScreen _chessScreen;
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly ScenarioSelector _scenarioSelector;

    public ListScenariosCommand(ChessScreen chessScreen, ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    {
        _chessScreen = chessScreen;
        _screenInvalidator = screenInvalidator;
        _scenarioSelector = scenarioSelector;
    }

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

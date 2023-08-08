namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class SelectScenarioCommand : ICommand
{
    private readonly ChessScreen _chessScreen;
    private readonly ScenarioSelector _scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator;

    public SelectScenarioCommand(ChessScreen chessScreen, ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    {
        _chessScreen = chessScreen;
        _scenarioSelector = scenarioSelector;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.Enter;

    public string Description => "[] Select scenario";

    public Task Execute()
    {
        _chessScreen.FinishSelectScenario(_scenarioSelector.SelectedScenario);
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}

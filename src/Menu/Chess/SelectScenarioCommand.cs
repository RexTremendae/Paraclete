namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class SelectScenarioCommand(ChessScreen chessScreen, ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ChessScreen _chessScreen = chessScreen;
    private readonly ScenarioSelector _scenarioSelector = scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.Enter;

    public string Description => "[] Select scenario";

    public Task Execute()
    {
        _chessScreen.FinishSelectScenario(_scenarioSelector.SelectedScenario);
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}

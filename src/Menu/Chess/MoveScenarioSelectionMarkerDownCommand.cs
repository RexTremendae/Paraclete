namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveScenarioSelectionMarkerDownCommand(ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ScenarioSelector _scenarioSelector = scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Move down";

    public Task Execute()
    {
        _scenarioSelector.MoveSelectionMarkerDown();
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Menu);
        return Task.CompletedTask;
    }
}

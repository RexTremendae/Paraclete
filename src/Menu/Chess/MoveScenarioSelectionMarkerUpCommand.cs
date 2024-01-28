namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveScenarioSelectionMarkerUpCommand(ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly ScenarioSelector _scenarioSelector = scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Move up";

    public Task Execute()
    {
        _scenarioSelector.MoveSelectionMarkerUp();
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}

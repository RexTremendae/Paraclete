namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveScenarioSelectionMarkerUpCommand : ICommand
{
    private readonly ScenarioSelector _scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator;

    public MoveScenarioSelectionMarkerUpCommand(ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    {
        _scenarioSelector = scenarioSelector;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Move up";

    public Task Execute()
    {
        _scenarioSelector.MoveSelectionMarkerUp();
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}

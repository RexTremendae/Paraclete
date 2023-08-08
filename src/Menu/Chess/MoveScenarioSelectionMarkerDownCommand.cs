namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveScenarioSelectionMarkerDownCommand : ICommand
{
    private readonly ScenarioSelector _scenarioSelector;
    private readonly ScreenInvalidator _screenInvalidator;

    public MoveScenarioSelectionMarkerDownCommand(ScenarioSelector scenarioSelector, ScreenInvalidator screenInvalidator)
    {
        _scenarioSelector = scenarioSelector;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Move down";

    public Task Execute()
    {
        _scenarioSelector.MoveSelectionMarkerDown();
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}

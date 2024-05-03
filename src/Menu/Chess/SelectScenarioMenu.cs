namespace Paraclete.Menu.Chess;

public class SelectScenarioMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(MoveScenarioSelectionMarkerUpCommand),
        typeof(MoveScenarioSelectionMarkerDownCommand),
        typeof(SelectScenarioCommand),
        typeof(CancelSelectScenarioCommand),
    ])
{
}

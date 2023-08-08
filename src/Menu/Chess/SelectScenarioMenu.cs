namespace Paraclete.Menu.Chess;

public class SelectScenarioMenu : MenuBase
{
    public SelectScenarioMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(MoveScenarioSelectionMarkerUpCommand),
        typeof(MoveScenarioSelectionMarkerDownCommand),
        typeof(SelectScenarioCommand),
        typeof(CancelSelectScenarioCommand),
    })
    {
    }
}

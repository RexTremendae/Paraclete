namespace Paraclete.Menu.Chess;

public class ChessMenu : MenuBase
{
    public ChessMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(MovePieceSelectionMarkerUpCommand),
        typeof(MovePieceSelectionMarkerDownCommand),
        typeof(MovePieceSelectionMarkerLeftCommand),
        typeof(MovePieceSelectionMarkerRightCommand),
        typeof(ListScenariosCommand),
    })
    {
    }
}

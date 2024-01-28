namespace Paraclete.Menu.Chess;

public class ChessMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(MovePieceSelectionMarkerUpCommand),
        typeof(MovePieceSelectionMarkerDownCommand),
        typeof(MovePieceSelectionMarkerLeftCommand),
        typeof(MovePieceSelectionMarkerRightCommand),
        typeof(ListScenariosCommand),
    })
{
}

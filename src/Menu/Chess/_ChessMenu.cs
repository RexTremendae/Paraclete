namespace Paraclete.Menu.Chess;

public class ChessMenu : MenuBase
{
    public ChessMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(MoveMarkerUpCommand),
        typeof(MoveMarkerDownCommand),
        typeof(MoveMarkerLeftCommand),
        typeof(MoveMarkerRightCommand),
    })
    {
    }
}

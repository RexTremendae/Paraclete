namespace Paraclete.Menu.Chess;

public class ChessMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(MoveUpCommand),
        typeof(MoveDownCommand),
        typeof(MoveLeftCommand),
        typeof(MoveRightCommand),
        typeof(AcceptCommand),
        typeof(CancelCommand),
        typeof(ListScenariosCommand),
        typeof(RotateBoardCommand),
    ])
{
}

namespace Paraclete.Menu.Chess.States;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Modules.Chess;

public class SelectPieceState(
    BoardSelectionService selectionService,
    PossibleMovesTracker possibleMovesTracker,
    ChessBoard board)
    : MenuStateBase<ChessMenuState, ChessMenuStateCommand>
{
    private readonly BoardSelectionService _selectionService = selectionService;
    private readonly PossibleMovesTracker _possibleMovesTracker = possibleMovesTracker;
    private readonly ChessBoard _board = board;

    public override ChessMenuState State => ChessMenuState.SelectPiece;

    public override IEnumerable<(ChessMenuStateCommand Command, bool PerformActions, ChessMenuState State)> Transitions =>
    [
        (ChessMenuStateCommand.Accept, true,  ChessMenuState.MovePiece),
    ];

    public override bool IsInitialState() => true;

#pragma warning disable IDE0010 // Populate switch
    public override void ExecuteCommand(ChessMenuStateCommand command)
    {
        switch (command)
        {
            case ChessMenuStateCommand.Up:
                _selectionService.MoveFromMarker((0, 1));
                break;

            case ChessMenuStateCommand.Down:
                _selectionService.MoveFromMarker((0, -1));
                break;

            case ChessMenuStateCommand.Left:
                _selectionService.MoveFromMarker((-1, 0));
                break;

            case ChessMenuStateCommand.Right:
                _selectionService.MoveFromMarker((1, 0));
                break;
        }
    }
#pragma warning restore

    public override IMenuActionResult<ChessMenuState> ExitAction()
    {
        var from = _selectionService.FromMarkerPosition;
        var canSelect = _board.GetPiece(from)?.Color == _board.CurrentPlayer
            && _possibleMovesTracker.GetPossibleMovesFrom(from).Any();

        return canSelect
            ? AcceptTransition
            : RejectTransition;
    }
}

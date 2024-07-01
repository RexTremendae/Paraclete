namespace Paraclete.Menu.Chess.States;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Modules.Chess;

public class MovePieceState(
    BoardSelectionService selectionService,
    PossibleMovesTracker possibleMovesTracker,
    ChessBoard board)
    : MenuStateBase<ChessMenuState, ChessMenuStateCommand>
{
    private readonly BoardSelectionService _selectionService = selectionService;
    private readonly PossibleMovesTracker _possibleMovesTracker = possibleMovesTracker;
    private readonly ChessBoard _board = board;

    public override ChessMenuState State => ChessMenuState.MovePiece;

    public override IEnumerable<(ChessMenuStateCommand Command, bool PerformActions, ChessMenuState State)> Transitions =>
    [
        (ChessMenuStateCommand.Accept,  true,  ChessMenuState.SelectPiece),
        (ChessMenuStateCommand.Cancel,  false, ChessMenuState.SelectPiece),
    ];

#pragma warning disable IDE0010 // Populate switch
    public override void ExecuteCommand(ChessMenuStateCommand command)
    {
        switch (command)
        {
            case ChessMenuStateCommand.Up:
                _selectionService.MoveToMarker((0, 1));
                break;

            case ChessMenuStateCommand.Down:
                _selectionService.MoveToMarker((0, -1));
                break;

            case ChessMenuStateCommand.Left:
                _selectionService.MoveToMarker((-1, 0));
                break;

            case ChessMenuStateCommand.Right:
                _selectionService.MoveToMarker((1, 0));
                break;
        }
    }
#pragma warning restore

    public override IMenuActionResult<ChessMenuState> EnterAction()
    {
        Move minMove = new();
        var minDist = 100;

        foreach (var move in _possibleMovesTracker.GetPossibleMovesFrom(_selectionService.FromMarkerPosition))
        {
            var distX = (move.From.X - move.To.X);
            var distY = (move.From.Y - move.To.Y);
            var dist = (distX * distX) + (distY * distY);

            if (dist < minDist)
            {
                minDist = dist;
                minMove = move;
            }
        }

        _selectionService.SetToMarkerPosition(minMove.To);

        return AcceptTransition;
    }

    public override IMenuActionResult<ChessMenuState> ExitAction()
    {
        var possibleMoves = _possibleMovesTracker
            .GetPossibleMovesFrom(_selectionService.FromMarkerPosition)
            .ToDictionary(key => key.To, value => value);

        if (!possibleMoves.TryGetValue(_selectionService.ToMarkerPosition, out var move))
        {
            return RejectTransition;
        }

        var isPromotion = _board.MovePiece(move.From, move.To);
        _selectionService.SetFromMarkerPosition(move.To);

        return isPromotion
            ? ForceTransition(ChessMenuState.PromotePiece, true)
            : AcceptTransition;
    }
}

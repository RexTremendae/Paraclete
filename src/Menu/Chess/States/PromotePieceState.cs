namespace Paraclete.Menu.Chess.States;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Modules.Chess;
using Paraclete.Modules.Chess.PieceDefinitions;

public class PromotePieceState(ChessBoard board, BoardSelectionService boardSelectionService)
    : MenuStateBase<ChessMenuState, ChessMenuStateCommand>
{
    private readonly BoardSelectionService _boardSelectionService = boardSelectionService;
    private readonly ChessBoard _board = board;

    public override ChessMenuState State => ChessMenuState.PromotePiece;

    public override IEnumerable<(ChessMenuStateCommand Command, bool PerformActions, ChessMenuState State)> Transitions =>
    [
        (ChessMenuStateCommand.Accept, true, ChessMenuState.SelectPiece),
    ];

    public override IMenuActionResult<ChessMenuState> EnterAction()
    {
        _board.PromotePiece(_boardSelectionService.ToMarkerPosition, new Queen());
        return ForceTransition(ChessMenuState.SelectPiece, true);
    }
}

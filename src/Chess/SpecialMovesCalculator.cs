namespace Paraclete.Chess;

using Paraclete.Chess.PieceDefinitions;

public class SpecialMovesCalculator
{
    private readonly MoveHistory _moveHistory;

    public SpecialMovesCalculator(MoveHistory moveHistory)
    {
        _moveHistory = moveHistory;
    }

    public IEnumerable<Move> GetPossibleMoves(GameState gameState)
    {
        var specialMoves = new List<Move>();
        specialMoves.AddRange(GetEnPassantMoves(gameState.Pieces));
        return specialMoves;
    }

    private IEnumerable<Move> GetEnPassantMoves(IReadOnlyDictionary<(int x, int y), ChessBoardPiece> pieces)
    {
        var nullableLastMove = _moveHistory.GetLastMove();
        if (nullableLastMove == null)
        {
            yield break;
        }

        var lastMove = nullableLastMove.Value;
        if (lastMove.pieceType != PieceType.Pawn ||
            Math.Abs(lastMove.to.y - lastMove.from.y) != 2)
        {
            yield break;
        }

        foreach (var dx in new[] { 1, -1 })
        {
            var x = lastMove.to.x + dx;
            if (x >= 0 && x < 8 &&
                pieces.TryGetValue((x, lastMove.to.y), out var piece) &&
                piece.definition.PieceType == PieceType.Pawn &&
                piece.color != lastMove.player)
            {
                yield return new Move(
                    piece.color,
                    PieceType.Pawn,
                    from: (x, lastMove.to.y),
                    to: (lastMove.to.x, lastMove.to.y + (piece.color == PlayerColor.White ? +1 : -1)),
                    null);
            }
        }
    }
}

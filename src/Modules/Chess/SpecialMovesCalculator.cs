namespace Paraclete.Modules.Chess;

using Paraclete.Modules.Chess.PieceDefinitions;

public class SpecialMovesCalculator(MoveHistory moveHistory)
{
    private readonly MoveHistory _moveHistory = moveHistory;

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
        if (lastMove.PieceType != PieceType.Pawn ||
            Math.Abs(lastMove.To.Y - lastMove.From.Y) != 2)
        {
            yield break;
        }

        foreach (var dx in new[] { 1, -1 })
        {
            var x = lastMove.To.X + dx;
            if (x >= 0 && x < 8 &&
                pieces.TryGetValue((x, lastMove.To.Y), out var piece) &&
                piece.Definition.PieceType == PieceType.Pawn &&
                piece.Color != lastMove.Player)
            {
                yield return new Move(
                    piece.Color,
                    PieceType.Pawn,
                    From: (x, lastMove.To.Y),
                    To: (lastMove.To.X, lastMove.To.Y + (piece.Color == PlayerColor.White ? +1 : -1)),
                    null);
            }
        }
    }
}

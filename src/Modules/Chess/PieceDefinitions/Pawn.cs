namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Pawn : PieceDefinition
{
    public override PieceType PieceType => PieceType.Pawn;
    public override char Representation => '*';

    protected override IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState)
    {
        var dir = (piece.Color == PlayerColor.White) ? +1 : -1;
        var x = position.X;
        var y = position.Y + dir;

        var otherPlayer = piece.Color.Swap();
        if (gameState.GetPiece((x + 1, y))?.Color == otherPlayer)
        {
            yield return (x + 1, y);
        }

        if (gameState.GetPiece((x - 1, y))?.Color == otherPlayer)
        {
            yield return (x - 1, y);
        }

        if (gameState.GetPiece((x, y)) != default)
        {
            yield break;
        }

        yield return (x, y);

        if (!piece.HasMoved)
        {
            y += dir;
            if (gameState.GetPiece((x, y)) != default)
            {
                yield break;
            }

            yield return (x, y);
        }
    }
}

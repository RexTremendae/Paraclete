namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Pawn : PieceDefinition
{
    public override PieceType PieceType => PieceType.Pawn;
    public override char Representation => '*';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        var dir = (piece.color == PlayerColor.White) ? +1 : -1;
        var x = position.x;
        var y = position.y + dir;

        var otherPlayer = piece.color.Swap();
        if (gameState.GetPiece((x + 1, y))?.color == otherPlayer)
        {
            yield return (x + 1, y);
        }

        if (gameState.GetPiece((x - 1, y))?.color == otherPlayer)
        {
            yield return (x - 1, y);
        }

        if (gameState.GetPiece((x, y)) != default)
        {
            yield break;
        }

        yield return (x, y);

        if (!piece.hasMoved)
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

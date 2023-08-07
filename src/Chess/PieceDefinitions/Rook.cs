namespace Paraclete.Chess.PieceDefinitions;

public class Rook : PieceDefinition
{
    public override PieceType PieceType => PieceType.Rook;
    public override char Representation => '□';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        foreach (var x in 0.To(8))
        {
            if (x != position.x)
            {
                yield return (x, position.y);
            }
        }

        foreach (var y in 0.To(8))
        {
            if (y != position.y)
            {
                yield return (position.x, y);
            }
        }
    }
}

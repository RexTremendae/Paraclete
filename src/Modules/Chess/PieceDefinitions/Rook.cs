namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Rook : PieceDefinition
{
    public override PieceType PieceType => PieceType.Rook;
    public override char Representation => '□';

    protected override IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState)
    {
        foreach (var x in 0.To(8))
        {
            if (x != position.X)
            {
                yield return (x, position.Y);
            }
        }

        foreach (var y in 0.To(8))
        {
            if (y != position.Y)
            {
                yield return (position.X, y);
            }
        }
    }
}

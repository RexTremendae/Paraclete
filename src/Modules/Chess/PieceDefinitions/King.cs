namespace Paraclete.Modules.Chess.PieceDefinitions;

public class King : PieceDefinition
{
    public override PieceType PieceType => PieceType.King;
    public override char Representation => '♰';

    protected override IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState)
    {
        foreach (var y in (-1).To(1, endIsInclusive: true))
        {
            foreach (var x in (-1).To(1, endIsInclusive: true))
            {
                yield return (position.X + x, position.Y + y);
            }
        }
    }
}

namespace Paraclete.Chess.PieceDefinitions;

public class King : PieceDefinition
{
    public override PieceType PieceType => PieceType.King;
    public override char Representation => '♰';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        foreach (var y in (-1).To(1, endIsInclusive: true))
        {
            foreach (var x in (-1).To(1, endIsInclusive: true))
            {
                yield return (position.x + x, position.y + y);
            }
        }
    }
}

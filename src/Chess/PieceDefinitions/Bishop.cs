namespace Paraclete.Chess.PieceDefinitions;

public class Bishop : PieceDefinition
{
    public override PieceType PieceType => PieceType.Bishop;
    public override char Representation => '∆';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        foreach (var d in (-7).To(7, endIsInclusive: true))
        {
            yield return (position.x + d, position.y + d);
            yield return (position.x - d, position.y + d);
        }
    }
}

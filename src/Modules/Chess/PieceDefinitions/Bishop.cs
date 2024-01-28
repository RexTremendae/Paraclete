namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Bishop : PieceDefinition
{
    public override PieceType PieceType => PieceType.Bishop;
    public override char Representation => '∆';

    protected override IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState)
    {
        foreach (var d in (-7).To(7, endIsInclusive: true))
        {
            yield return (position.X + d, position.Y + d);
            yield return (position.X - d, position.Y + d);
        }
    }
}

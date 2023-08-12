namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Knight : PieceDefinition
{
    public override PieceType PieceType => PieceType.Knight;
    public override char Representation => '&';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        foreach (var x in new[] { -2, -1, 1, 2 })
        {
            foreach (var y in new[] { -2, -1, 1, 2 })
            {
                if (x * x != y * y)
                {
                    yield return (position.x + x, position.y + y);
                }
            }
        }
    }
}

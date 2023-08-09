namespace Paraclete.Chess.Scenarios;

using Paraclete.Chess.PieceDefinitions;

public class NewGame : IScenario
{
    public virtual string Name => "New Game";
    public virtual PlayerColor CurrentPlayer => PlayerColor.White;

    public virtual IEnumerable<((int x, int y) position, ChessBoardPiece piece)> GetPieces()
    {
        int x = 0;
        int y = 0;

        yield return ((x++, y), new (new Rook(),   PlayerColor.White, false));
        yield return ((x++, y), new (new Knight(), PlayerColor.White, false));
        yield return ((x++, y), new (new Bishop(), PlayerColor.White, false));
        yield return ((x++, y), new (new Queen(),  PlayerColor.White, false));
        yield return ((x++, y), new (new King(),   PlayerColor.White, false));
        yield return ((x++, y), new (new Bishop(), PlayerColor.White, false));
        yield return ((x++, y), new (new Knight(), PlayerColor.White, false));
        yield return ((x++, y), new (new Rook(),   PlayerColor.White, false));

        y++;
        for (x = 0; x < 8; x++)
        {
            yield return ((x, y), new (new Pawn(), PlayerColor.White, false));
        }

        y = 6;
        for (x = 0; x < 8; x++)
        {
            yield return ((x, y), new (new Pawn(), PlayerColor.Black, false));
        }

        x = 0;
        y++;

        yield return ((x++, y), new (new Rook(),   PlayerColor.Black, false));
        yield return ((x++, y), new (new Knight(), PlayerColor.Black, false));
        yield return ((x++, y), new (new Bishop(), PlayerColor.Black, false));
        yield return ((x++, y), new (new Queen(),  PlayerColor.Black, false));
        yield return ((x++, y), new (new King(),   PlayerColor.Black, false));
        yield return ((x++, y), new (new Bishop(), PlayerColor.Black, false));
        yield return ((x++, y), new (new Knight(), PlayerColor.Black, false));
        yield return ((x++, y), new (new Rook(),   PlayerColor.Black, false));
    }
}

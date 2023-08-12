namespace Paraclete.Modules.Chess.Scenarios;

using Paraclete.Modules.Chess.PieceDefinitions;

public class EnPassant : NewGame
{
    public override string Name => "En Passant";
    public override PlayerColor CurrentPlayer => PlayerColor.Black;

    public override IEnumerable<((int x, int y) position, ChessBoardPiece piece)> GetPieces()
    {
        var pieces = base.GetPieces().ToDictionary(key => key.position, value => value.piece);

        pieces.Remove((3, 6));
        pieces[(3, 3)] = new (new Pawn(), PlayerColor.Black, true);

        pieces.Remove((7, 6));
        pieces[(7, 3)] = new (new Pawn(), PlayerColor.Black, true);

        pieces.Remove((5, 1));
        pieces[(5, 4)] = new (new Pawn(), PlayerColor.White, true);

        pieces.Remove((1, 1));
        pieces[(1, 4)] = new (new Pawn(), PlayerColor.White, true);

        return pieces.Select(_ => (_.Key, _.Value));
    }
}

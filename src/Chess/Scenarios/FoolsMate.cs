namespace Paraclete.Chess.Scenarios;

using Paraclete.Chess.PieceDefinitions;

public class FoolsMate : NewGame
{
    public override string Name => "Fool's Mate";
    public override PlayerColor CurrentPlayer => PlayerColor.Black;

    public override IEnumerable<((int x, int y) position, ChessBoardPiece piece)> GetPieces()
    {
        var pieces = base.GetPieces().ToDictionary(key => key.position, value => value.piece);

        pieces.Remove((4, 6));

        pieces[(4, 4)] = new (new Pawn(), PlayerColor.Black, true);

        pieces.Remove((5, 1));
        pieces.Remove((6, 1));

        pieces[(5, 2)] = new (new Pawn(), PlayerColor.White, true);
        pieces[(6, 3)] = new (new Pawn(), PlayerColor.White, true);

        return pieces.Select(_ => (_.Key, _.Value));
    }
}

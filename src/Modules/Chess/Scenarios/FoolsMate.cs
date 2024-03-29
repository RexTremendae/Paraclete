namespace Paraclete.Modules.Chess.Scenarios;

using Paraclete.Modules.Chess.PieceDefinitions;

public class FoolsMate : NewGame
{
    public override string Name => "Fool's Mate";
    public override PlayerColor CurrentPlayer => PlayerColor.Black;

    public override IEnumerable<((int X, int Y) Position, ChessBoardPiece Piece)> GetPieces()
    {
        var pieces = base.GetPieces().ToDictionary(key => key.Position, value => value.Piece);

        pieces.Remove((4, 6));

        pieces[(4, 4)] = new(new Pawn(), PlayerColor.Black, true);

        pieces.Remove((5, 1));
        pieces.Remove((6, 1));

        pieces[(5, 2)] = new(new Pawn(), PlayerColor.White, true);
        pieces[(6, 3)] = new(new Pawn(), PlayerColor.White, true);

        return pieces.Select(_ => (_.Key, _.Value));
    }
}

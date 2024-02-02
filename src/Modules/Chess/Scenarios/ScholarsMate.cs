namespace Paraclete.Modules.Chess.Scenarios;

using Paraclete.Modules.Chess.PieceDefinitions;

public class ScholarsMate : NewGame
{
    public override string Name => "Scholar's Mate";
    public override PlayerColor CurrentPlayer => PlayerColor.White;

    public override IEnumerable<((int X, int Y) Position, ChessBoardPiece Piece)> GetPieces()
    {
        var pieces = base.GetPieces().ToDictionary(key => key.Position, value => value.Piece);

        pieces.Remove((1, 7));
        pieces.Remove((6, 7));
        pieces.Remove((4, 6));

        pieces[(2, 5)] = new(new Knight(), PlayerColor.Black, true);
        pieces[(5, 5)] = new(new Knight(), PlayerColor.Black, true);
        pieces[(4, 4)] = new(new Pawn(),   PlayerColor.Black, true);

        pieces.Remove((4, 1));
        pieces.Remove((3, 0));
        pieces.Remove((5, 0));

        pieces[(4, 3)] = new(new Pawn(),   PlayerColor.White, true);
        pieces[(2, 3)] = new(new Bishop(), PlayerColor.White, true);
        pieces[(7, 4)] = new(new Queen(),  PlayerColor.White, true);

        return pieces.Select(_ => (_.Key, _.Value));
    }
}

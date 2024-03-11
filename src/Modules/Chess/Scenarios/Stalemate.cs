namespace Paraclete.Modules.Chess.Scenarios;

using Paraclete.Modules.Chess.PieceDefinitions;

public class Stalemate : IScenario
{
    public string Name => "Stalemate";
    public PlayerColor CurrentPlayer => PlayerColor.White;

    public Dictionary<PlayerColor, IEnumerable<ChessBoardPiece>> CapturedPieces => new()
    {
        {
            PlayerColor.Black,
            new[]
            {
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Bishop(), PlayerColor.Black, false),
                new ChessBoardPiece(new Bishop(), PlayerColor.Black, false),
                new ChessBoardPiece(new Knight(), PlayerColor.Black, false),
                new ChessBoardPiece(new Knight(), PlayerColor.Black, false),
                new ChessBoardPiece(new Rook(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Rook(),   PlayerColor.Black, false),
                new ChessBoardPiece(new Queen(),  PlayerColor.Black, false),
            }
        },
        {
            PlayerColor.White,
            new[]
            {
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Pawn(),   PlayerColor.White, false),
                new ChessBoardPiece(new Bishop(), PlayerColor.White, false),
                new ChessBoardPiece(new Bishop(), PlayerColor.White, false),
                new ChessBoardPiece(new Knight(), PlayerColor.White, false),
                new ChessBoardPiece(new Knight(), PlayerColor.White, false),
                new ChessBoardPiece(new Rook(),   PlayerColor.White, false),
                new ChessBoardPiece(new Rook(),   PlayerColor.White, false),
                new ChessBoardPiece(new Queen(),  PlayerColor.White, false),
            }
        },
    };

    public IEnumerable<((int X, int Y) Position, ChessBoardPiece Piece)> GetPieces() =>
    [
        ((3, 4), new(new King(), PlayerColor.White, true)),
        ((3, 7), new(new King(), PlayerColor.Black, true)),
        ((3, 6), new(new Pawn(), PlayerColor.White, true)),
    ];
}

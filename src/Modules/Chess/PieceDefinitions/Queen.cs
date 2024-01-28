namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Queen : PieceDefinition
{
    public override PieceType PieceType => PieceType.Queen;
    public override char Representation => '¤';

    protected override IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState)
    {
        return new Rook().GetPossibleMoves(position, gameState, validateAndSort: false)
            .Union(new Bishop().GetPossibleMoves(position, gameState, validateAndSort: false));
    }
}

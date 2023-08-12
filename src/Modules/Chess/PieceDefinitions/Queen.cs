namespace Paraclete.Modules.Chess.PieceDefinitions;

public class Queen : PieceDefinition
{
    public override PieceType PieceType => PieceType.Queen;
    public override char Representation => '¤';

    protected override IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState)
    {
        return new Rook().GetPossibleMoves(position, gameState, validateAndSort: false)
            .Union(new Bishop().GetPossibleMoves(position, gameState, validateAndSort: false));
    }
}

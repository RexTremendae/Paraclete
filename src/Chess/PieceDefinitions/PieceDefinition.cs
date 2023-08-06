namespace Paraclete.Chess.PieceDefinitions;

public abstract class PieceDefinition
{
    public virtual PieceType PieceType => PieceType.Pawn;
    public virtual char Representation => ' ';

    public IEnumerable<(int x, int y)> GetPossibleMoves((int x, int y) position, GameState gameState, bool validateAndSort = true)
    {
        var sourcePiece = gameState.GetPiece(position)
            ?? throw new ArgumentNullException("pieceDefinition");

        var possibleMoves = GetPossibleMovesForPiece(sourcePiece, position, gameState);

        return validateAndSort
            ? ValidateAndSort(sourcePiece, position, gameState, possibleMoves)
            : possibleMoves;
    }

    protected abstract IEnumerable<(int x, int y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int x, int y) position, GameState gameState);

    private IEnumerable<(int x, int y)> ValidateAndSort(ChessBoardPiece piece, (int x, int y) position, GameState gameState, IEnumerable<(int x, int y)> possibleMoves)
    {
        var moves = possibleMoves.ToHashSet();
        foreach (var (dirX, dirY) in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, -1), (1, -1), (-1, 1), (1, 1) } )
        {
            var curr = position;
            var blocked = false;

            1.To(7, endIsInclusive: true).Foreach(_ =>
            {
                curr = (curr.x + dirX, curr.y + dirY);

                if (blocked)
                {
                    moves.Remove(curr);
                }
                else
                {
                    var currPiece = gameState.GetPiece(curr);
                    if (currPiece != default)
                    {
                        blocked = true;
                        if (currPiece!.Value.color == piece.color)
                        {
                            moves.Remove(curr);
                        }
                    }
                }
            });
        }

        foreach (var (x, y) in moves.OrderBy(_ => _.y).ThenBy(_ => _.x))
        {
            if (x < 0 || y < 0 || x >= 8 || y >= 8)
            {
                continue;
            }

            var destinationPiece = gameState.GetPiece((x, y));
            if (destinationPiece?.color == piece.color)
            {
                continue;
            }

            yield return (x, y);
        }
    }
}

namespace Paraclete.Modules.Chess.PieceDefinitions;

public abstract class PieceDefinition
{
    public virtual PieceType PieceType => PieceType.Pawn;
    public virtual char Representation => ' ';

    public IEnumerable<(int X, int Y)> GetPossibleMoves((int X, int Y) position, GameState gameState, bool validateAndSort = true)
    {
        var sourcePiece = gameState.GetPiece(position)
            ?? throw new ArgumentException($"No piece at position {position}", nameof(position));

        var possibleMoves = GetPossibleMovesForPiece(sourcePiece, position, gameState);

        return validateAndSort
            ? ValidateAndSort(sourcePiece, position, gameState, possibleMoves)
            : possibleMoves;
    }

    protected abstract IEnumerable<(int X, int Y)> GetPossibleMovesForPiece(ChessBoardPiece piece, (int X, int Y) position, GameState gameState);

    private static IEnumerable<(int X, int Y)> ValidateAndSort(
        ChessBoardPiece piece,
        (int X, int Y) position,
        GameState gameState,
        IEnumerable<(int X, int Y)> possibleMoves)
    {
        var moves = possibleMoves.ToHashSet();
        foreach (var (dirX, dirY) in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, -1), (1, -1), (-1, 1), (1, 1) } )
        {
            var curr = position;
            var blocked = false;

            1.To(7, endIsInclusive: true).Foreach(_ =>
            {
                curr = (curr.X + dirX, curr.Y + dirY);

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
                        if (currPiece!.Value.Color == piece.Color)
                        {
                            moves.Remove(curr);
                        }
                    }
                }
            });
        }

        foreach (var (x, y) in moves.OrderBy(_ => _.Y).ThenBy(_ => _.X))
        {
            if (x < 0 || y < 0 || x >= 8 || y >= 8)
            {
                continue;
            }

            var destinationPiece = gameState.GetPiece((x, y));
            if (destinationPiece?.Color == piece.Color)
            {
                continue;
            }

            yield return (x, y);
        }
    }
}

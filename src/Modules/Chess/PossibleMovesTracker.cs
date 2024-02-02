namespace Paraclete.Modules.Chess;

public class PossibleMovesTracker(SpecialMovesCalculator specialMovesCalculator)
{
    private readonly SpecialMovesCalculator _specialMovesCalculator = specialMovesCalculator;
    private readonly Dictionary<(int X, int Y), List<Move>> _possibleMovesFrom = [];
    private readonly Dictionary<(int X, int Y), List<Move>> _possibleMovesTo = [];

    public Move[] GetPossibleMovesFrom((int X, int Y) position)
    {
        return _possibleMovesFrom.TryGetValue(position, out var moves)
            ? [.. moves]
            : [];
    }

    public Move[] GetPossibleMovesTo((int X, int Y) position)
    {
        return _possibleMovesTo.TryGetValue(position, out var moves)
            ? [.. moves]
            : [];
    }

    public void Recalculate(GameState gameState)
    {
        _possibleMovesFrom.Clear();
        _possibleMovesTo.Clear();

        foreach (var (from, piece) in gameState.Pieces)
        {
            foreach (var to in piece.Definition.GetPossibleMoves(from, gameState))
            {
                var move = new Move(Player: piece.Color, PieceType: piece.Definition.PieceType, From: from, To: to, CapturedPiece: gameState.GetPiece(to));
                AddMove(move);
            }
        }

        foreach (var move in _specialMovesCalculator.GetPossibleMoves(gameState))
        {
            AddMove(move);
        }
    }

    private static void AddMoveTo(Dictionary<(int X, int Y), List<Move>> possibleMoves, (int X, int Y) key, Move move)
    {
        if (!possibleMoves.TryGetValue(key, out var moveList))
        {
            moveList = [];
            possibleMoves.Add(key, moveList);
        }

        moveList.Add(move);
    }

    private void AddMove(Move move)
    {
        AddMoveTo(_possibleMovesFrom, move.From, move);
        AddMoveTo(_possibleMovesTo, move.To, move);
    }
}

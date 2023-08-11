namespace Paraclete.Chess;

public class PossibleMovesTracker
{
    private readonly SpecialMovesCalculator _specialMovesCalculator;
    private readonly Dictionary<(int x, int y), List<Move>> _possibleMovesFrom = new ();
    private readonly Dictionary<(int x, int y), List<Move>> _possibleMovesTo = new ();

    public PossibleMovesTracker(SpecialMovesCalculator specialMovesCalculator)
    {
        _specialMovesCalculator = specialMovesCalculator;
    }

    public Move[] GetPossibleMovesFrom((int x, int y) position)
    {
        return _possibleMovesFrom.TryGetValue(position, out var moves)
            ? moves.ToArray()
            : Array.Empty<Move>();
    }

    public Move[] GetPossibleMovesTo((int x, int y) position)
    {
        return _possibleMovesTo.TryGetValue(position, out var moves)
            ? moves.ToArray()
            : Array.Empty<Move>();
    }

    public void Recalculate(GameState gameState)
    {
        _possibleMovesFrom.Clear();
        _possibleMovesTo.Clear();

        foreach (var (from, piece) in gameState.Pieces)
        {
            foreach (var to in piece.definition.GetPossibleMoves(from, gameState))
            {
                var move = new Move(player: piece.color, pieceType: piece.definition.PieceType, from: from, to: to, capturedPiece: gameState.GetPiece(to));
                AddMove(move);
            }
        }

        foreach (var move in _specialMovesCalculator.GetPossibleMoves(gameState))
        {
            AddMove(move);
        }
    }

    private static void AddMoveTo(Dictionary<(int x, int y), List<Move>> possibleMoves, (int x, int y) key, Move move)
    {
        if (!possibleMoves.TryGetValue(key, out var moveList))
        {
            moveList = new ();
            possibleMoves.Add(key, moveList);
        }

        moveList.Add(move);
    }

    private void AddMove(Move move)
    {
        AddMoveTo(_possibleMovesFrom, move.from, move);
        AddMoveTo(_possibleMovesTo, move.to, move);
    }
}

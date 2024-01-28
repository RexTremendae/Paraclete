namespace Paraclete.Modules.Chess;

public class MoveHistory
{
    private readonly List<(int Count, Move Move)> _moves = [];

    public void Clear()
    {
        _moves.Clear();
    }

    public void Add(Move move)
    {
        _moves.Add((_moves.Count + 1, move));
    }

    public Move? GetLastMove()
    {
        return _moves.Any()
            ? _moves.Last().Move
            : null;
    }

    public IEnumerable<(int Count, Move Move)> GetLastMoves(int maxMoves)
    {
        return _moves.Count <= maxMoves
            ? _moves
            : _moves.Skip(_moves.Count - maxMoves);
    }
}

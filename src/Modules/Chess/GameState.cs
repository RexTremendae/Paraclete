namespace Paraclete.Modules.Chess;

using System.Collections.ObjectModel;

public class GameState
{
    private readonly ChessBoardPiece?[] _pieces;
    private ReadOnlyDictionary<(int x, int y), ChessBoardPiece>? _piecesLookup;

    public GameState(IEnumerable<((int x, int y) position, ChessBoardPiece piece)> pieces)
    {
        _pieces = new ChessBoardPiece?[64];
        foreach (var (position, piece) in pieces)
        {
            _pieces[ToIndex(position)] = piece;
        }
    }

    public Dictionary<(int x, int y), ChessBoardPiece> PiecesClone
        => Pieces.ToDictionary(key => key.Key, value => value.Value);

    public IReadOnlyDictionary<(int x, int y), ChessBoardPiece> Pieces
    {
        get
        {
            if (_piecesLookup != null)
            {
                return _piecesLookup;
            }

            var dict = new Dictionary<(int, int), ChessBoardPiece>();

            int index = 0;
            foreach (var y in 0.To(8))
            {
                foreach (var x in 0.To(8))
                {
                    var piece = _pieces[index++];
                    if (piece.HasValue)
                    {
                        dict.Add((x, y), piece.Value);
                    }
                }
            }

            _piecesLookup = new (dict);
            return _piecesLookup;
        }
    }

    public ChessBoardPiece? GetPiece((int x, int y) pos)
    {
        return (pos.x < 0 || pos.y < 0 || pos.x >= 8 || pos.y >= 8)
            ? null
            : _pieces[ToIndex(pos)];
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        0.To(64).Foreach(_ =>
        {
            var piece = _pieces[_];
            if (piece != null)
            {
                hashCode.Add((_, piece.Value.definition.PieceType, piece.Value.color));
            }
        });

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not GameState other)
        {
            return false;
        }

        foreach (var pidx in 0.To(64))
        {
            var p1n = _pieces[pidx];
            var p2n = other._pieces[pidx];

            if (p1n == null && p2n == null)
            {
                continue;
            }

            if (p1n == null || p2n == null)
            {
                return false;
            }

            var p1 = p1n.Value;
            var p2 = p2n.Value;

            if (p1.color != p2.color)
            {
                return false;
            }

            if (p1.definition.PieceType != p2.definition.PieceType)
            {
                return false;
            }
        }

        return true;
    }

    private static int ToIndex((int x, int y) pos)
    {
        return (pos.y * 8) + pos.x;
    }
}

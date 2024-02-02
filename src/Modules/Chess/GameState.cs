namespace Paraclete.Modules.Chess;

using System.Collections.ObjectModel;

public class GameState
{
    private readonly ChessBoardPiece?[] _pieces;
    private ReadOnlyDictionary<(int X, int Y), ChessBoardPiece>? _piecesLookup;

    public GameState(IEnumerable<((int X, int Y) Position, ChessBoardPiece Piece)> pieces)
    {
        _pieces = new ChessBoardPiece?[64];
        foreach (var (position, piece) in pieces)
        {
            _pieces[ToIndex(position)] = piece;
        }
    }

    public Dictionary<(int X, int Y), ChessBoardPiece> PiecesClone
        => Pieces.ToDictionary(key => key.Key, value => value.Value);

    public IReadOnlyDictionary<(int X, int Y), ChessBoardPiece> Pieces
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

            _piecesLookup = new(dict);
            return _piecesLookup;
        }
    }

    public ChessBoardPiece? GetPiece((int X, int Y) pos)
    {
        return (pos.X < 0 || pos.Y < 0 || pos.X >= 8 || pos.Y >= 8)
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
                hashCode.Add((_, piece.Value.Definition.PieceType, piece.Value.Color));
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

            if (p1.Color != p2.Color)
            {
                return false;
            }

            if (p1.Definition.PieceType != p2.Definition.PieceType)
            {
                return false;
            }
        }

        return true;
    }

    private static int ToIndex((int X, int Y) pos)
    {
        return (pos.Y * 8) + pos.X;
    }
}

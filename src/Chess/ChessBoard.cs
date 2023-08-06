namespace Paraclete.Chess;

using Paraclete.Chess.PieceDefinitions;
using Paraclete.Screens.Chess;

public readonly record struct ChessBoardPiece
(
    PieceDefinition definition,
    PlayerColor color,
    bool hasMoved
);

public class ChessBoard
{
    private readonly GameState _gameState;

    private readonly Dictionary<PlayerColor, List<ChessBoardPiece>> _capturedPieces = new ()
    {
        { PlayerColor.White, new () },
        { PlayerColor.Black, new () },
    };

    public ChessBoard()
    {
        _gameState = new (Enumerable.Empty<((int x, int y), ChessBoardPiece)>());
    }

    public PlayerColor CurrentPlayer { get; private set; }
    public bool IsCheck { get; private set; }

    public ChessBoardPiece? GetPiece(int x, int y)
    {
        return GetPiece((x, y));
    }

    public ChessBoardPiece? GetPiece((int x, int y) pos)
    {
        return (pos.x < 0 || pos.y < 0 || pos.x >= 8 || pos.y >= 8)
            ? null
            : _gameState.GetPiece(pos);
    }

    public ChessBoardPiece[] GetCapturedPieces(PlayerColor color)
    {
        return _capturedPieces[color].ToArray();
    }

    public void PromotePiece((int x, int y) position, PieceDefinition promoteToPiece)
    {
        var piecesClone = _gameState.PiecesClone;
        if (!piecesClone.TryGetValue(position, out var piece))
        {
            throw new InvalidOperationException($"No piece found at position ({position.x}, {position.y})");
        }

        var pieceType = piece.definition.PieceType;
        if (pieceType != PieceType.Pawn)
        {
            throw new InvalidOperationException($"Piece at position ({position.x}, {position.y}) is a {pieceType} which cannot be promoted.");
        }

        if (position.y != 0 && position.y != 7)
        {
            throw new InvalidOperationException($"Pawn at position ({position.x}, {position.y}) has not reached the end of the board.");
        }

        piecesClone[position] = new ChessBoardPiece(definition: promoteToPiece, color: piece.color, hasMoved: false);
        CurrentPlayer = CurrentPlayer.Swap();

        UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));
    }

    public bool MovePiece((int x, int y) startPosition, (int x, int y) endPosition)
    {
        var piecesClone = _gameState.PiecesClone;
        if (!piecesClone.TryGetValue(startPosition, out var piece))
        {
            throw new InvalidOperationException($"No piece found at position ({startPosition.x}, {startPosition.y})");
        }

        ChessBoardPiece? capturedPiece = null;
        if (piecesClone.TryGetValue(endPosition, out var endPiece))
        {
            capturedPiece = endPiece;
            _capturedPieces[endPiece.color].Add(endPiece);
        }

        piecesClone[endPosition] = piece with { hasMoved = true };
        piecesClone.Remove(startPosition);
/*
        _moveHistory.Add(new Move(
            player: CurrentPlayer,
            pieceType: piece.Definition.PieceType,
            from: startPosition,
            to: endPosition,
            capturedPiece: capturedPiece));
*/
        if (piece.definition.PieceType == PieceType.Pawn && (endPosition.y == 0 || endPosition.y == 7))
        {
            UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));
            return true;
        }

/*
        if (piece.Definition.PieceType == PieceType.King)
        {
            _kingTracker[CurrentPlayer] = endPosition;
        }
*/

        CurrentPlayer = CurrentPlayer.Swap();
        UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));

        return false;
    }

    public (int x, int y) FindNextPieceLocation((int x, int y) position, int direction, PlayerColor color)
    {
        var index = (position.y * 8) + position.x;

        for (; ; )
        {
            index += direction;

            index =
                (index < 0) ? 63 :
                (index > 63) ? 0 : index;

            var next = (index % 8, index / 8);

            if (_gameState.Pieces.TryGetValue(next, out var piece) && piece.color == color)
            {
                return next;
            }
        }
    }

/*
    public (int x, int y) FindNextPossibleMoveToPosition((int x, int y) position, (int x, int y) piecePosition, int direction)
    {
        var index = (position.y * 8) + position.x;

        var moves = _possibleMovesTracker
            .GetPossibleMovesFrom(piecePosition)
            .Select(_ => _.to)
            .ToHashSet();

        if (!moves.Any())
        {
            return position;
        }

        for (; ; )
        {
            index += direction;

            index =
                (index < 0) ? 63 :
                (index > 63) ? 0 : index;

            var next = (index % 8, index / 8);

            if (moves.Contains(next))
            {
                return next;
            }
        }
    }

*/

    private void UpdateGameState(IEnumerable<((int x, int y) position, ChessBoardPiece piece)> pieces)
    {
        // TODO: Avoid unused parameter until everything is properly implemented:
        if (pieces.First().position.x == 1)
        {
        }

/*
        _gameState = new GameState(pieces);

        _possibleMovesTracker.Recalculate(_gameState);
        var possibleMovesToKing = _possibleMovesTracker.GetPossibleMovesTo(_kingTracker[CurrentPlayer]);
        var otherPlayer = CurrentPlayer.Swap();
        IsCheck = possibleMovesToKing.Any(_ => _gameState.Pieces[_.from].Color == otherPlayer);
*/
    }

/*
    private int ToIndex((int x, int y) pos)
    {
        return (pos.y * 8) + pos.x;
    }
*/
}

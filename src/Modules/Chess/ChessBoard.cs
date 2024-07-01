namespace Paraclete.Modules.Chess;

using System.Threading.Tasks;
using Paraclete.Modules.Chess.PieceDefinitions;
using Paraclete.Modules.Chess.Scenarios;

public readonly record struct ChessBoardPiece
(
    PieceDefinition Definition,
    PlayerColor Color,
    bool HasMoved
);

public class ChessBoard(PossibleMovesTracker possibleMovesTracker, MoveHistory moveHistory, BoardSelectionService boardSelectionService)
    : IInitializer
{
    private readonly Dictionary<PlayerColor, List<ChessBoardPiece>> _capturedPieces = new()
    {
        { PlayerColor.White, [] },
        { PlayerColor.Black, [] },
    };

    private readonly Dictionary<PlayerColor, (int X, int Y)> _kingTracker = [];
    private readonly PossibleMovesTracker _possibleMovesTracker = possibleMovesTracker;
    private readonly MoveHistory _moveHistory = moveHistory;
    private readonly BoardSelectionService _boardSelectionService = boardSelectionService;

    private GameState _gameState = new([]);

    public PlayerColor CurrentPlayer { get; private set; }
    public bool IsCheck { get; private set; }

    public ChessBoardPiece? GetPiece(int x, int y)
    {
        return GetPiece((x, y));
    }

    public ChessBoardPiece? GetPiece((int X, int Y) pos)
    {
        return (pos.X < 0 || pos.Y < 0 || pos.X >= 8 || pos.Y >= 8)
            ? null
            : _gameState.GetPiece(pos);
    }

    public ChessBoardPiece[] GetCapturedPieces(PlayerColor color)
    {
        return [.. _capturedPieces[color]];
    }

    public Task Initialize(IServiceProvider services)
    {
        InitializeScenario<NewGame>();
        return Task.CompletedTask;
    }

    public void InitializeScenario(IScenario scenario)
    {
        var pieces = new List<((int, int) Position, ChessBoardPiece Piece)>();
        foreach (var (position, piece) in scenario.GetPieces())
        {
            pieces.Add((position, piece));

            if (piece.Definition.PieceType == PieceType.King)
            {
                _kingTracker[piece.Color] = position;
            }
        }

        foreach (var color in new[] { PlayerColor.Black, PlayerColor.White })
        {
            _capturedPieces[color].Clear();
            foreach (var piece in scenario.CapturedPieces[color])
            {
                _capturedPieces[color].Add(piece);
            }
        }

        UpdateGameState(pieces);
        CurrentPlayer = scenario.CurrentPlayer;
        _boardSelectionService.SetFromMarkerPosition(pieces.First(_ => _.Piece.Color == CurrentPlayer).Position);
    }

    public void PromotePiece((int X, int Y) position, PieceDefinition promoteToPiece)
    {
        var piecesClone = _gameState.PiecesClone;
        if (!piecesClone.TryGetValue(position, out var piece))
        {
            throw new InvalidOperationException($"No piece found at position ({position.X}, {position.Y})");
        }

        var pieceType = piece.Definition.PieceType;
        if (pieceType != PieceType.Pawn)
        {
            throw new InvalidOperationException($"Piece at position ({position.X}, {position.Y}) is a {pieceType} which cannot be promoted.");
        }

        if (position.Y != 0 && position.Y != 7)
        {
            throw new InvalidOperationException($"Pawn at position ({position.X}, {position.Y}) has not reached the end of the board.");
        }

        piecesClone[position] = new ChessBoardPiece(Definition: promoteToPiece, Color: piece.Color, HasMoved: false);
        CurrentPlayer = CurrentPlayer.Swap();

        UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));
    }

    public bool MovePiece((int X, int Y) startPosition, (int X, int Y) endPosition)
    {
        var piecesClone = _gameState.PiecesClone;
        if (!piecesClone.TryGetValue(startPosition, out var piece))
        {
            throw new InvalidOperationException($"No piece found at position ({startPosition.X}, {startPosition.Y})");
        }

        ChessBoardPiece? capturedPiece = null;
        if (piecesClone.TryGetValue(endPosition, out var endPiece))
        {
            capturedPiece = endPiece;
            _capturedPieces[endPiece.Color].Add(endPiece);
        }

        piecesClone[endPosition] = piece with { HasMoved = true };
        piecesClone.Remove(startPosition);

        _moveHistory.Add(new Move(
            Player: CurrentPlayer,
            PieceType: piece.Definition.PieceType,
            From: startPosition,
            To: endPosition,
            CapturedPiece: capturedPiece));

        if (piece.Definition.PieceType == PieceType.Pawn && (endPosition.Y == 0 || endPosition.Y == 7))
        {
            UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));
            return true;
        }

        if (piece.Definition.PieceType == PieceType.King)
        {
            _kingTracker[CurrentPlayer] = endPosition;
        }

        CurrentPlayer = CurrentPlayer.Swap();
        UpdateGameState(piecesClone.Select(_ => (_.Key, _.Value)));

        return false;
    }

    public (int X, int Y) FindNextPieceLocation((int X, int Y) position, int direction, PlayerColor color)
    {
        var index = (position.Y * 8) + position.X;

        while (true)
        {
            index += direction;

            index =
                (index < 0) ? 63 :
                (index > 63) ? 0 :
                index;

            var next = (index % 8, index / 8);

            if (_gameState.Pieces.TryGetValue(next, out var piece) && piece.Color == color)
            {
                return next;
            }
        }
    }

    private void InitializeScenario<T>()
        where T : IScenario
    {
        InitializeScenario(Activator.CreateInstance<T>());
    }

    private void UpdateGameState(IEnumerable<((int X, int Y) Position, ChessBoardPiece Piece)> pieces)
    {
        _gameState = new GameState(pieces);

        _possibleMovesTracker.Recalculate(_gameState);
        var possibleMovesToKing = _possibleMovesTracker.GetPossibleMovesTo(_kingTracker[CurrentPlayer]);
        var otherPlayer = CurrentPlayer.Swap();
        IsCheck = possibleMovesToKing.Any(_ => _gameState.Pieces[_.From].Color == otherPlayer);
    }
}

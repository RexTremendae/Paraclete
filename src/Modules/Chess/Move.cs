namespace Paraclete.Modules.Chess;

using Paraclete.Modules.Chess.PieceDefinitions;

public readonly record struct Move
(
    PlayerColor Player,
    PieceType PieceType,
    (int X, int Y) From,
    (int X, int Y) To,
    ChessBoardPiece? CapturedPiece
);

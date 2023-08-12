namespace Paraclete.Modules.Chess;

using Paraclete.Modules.Chess.PieceDefinitions;

public readonly record struct Move
(
    PlayerColor player,
    PieceType pieceType,
    (int x, int y) from,
    (int x, int y) to,
    ChessBoardPiece? capturedPiece
);
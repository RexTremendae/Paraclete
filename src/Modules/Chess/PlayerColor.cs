namespace Paraclete.Modules.Chess;

public enum PlayerColor
{
    White,
    Black,
}

public static class PlayerColorExtensions
{
    public static PlayerColor Swap(this PlayerColor playerColor)
    {
        return playerColor == PlayerColor.White
            ? PlayerColor.Black
            : PlayerColor.White;
    }
}
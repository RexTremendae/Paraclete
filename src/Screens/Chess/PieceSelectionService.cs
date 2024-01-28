namespace Paraclete.Screens.Chess;

public class PieceSelectionService(ScreenInvalidator screenInvalidator, Settings settings)
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly Settings.ChessSettings _settings = settings.Chess;

    public (int X, int Y) MarkerPosition { get; private set; }

    public void MoveSelection(int deltaX, int deltaY)
    {
        (deltaX, deltaY) = (GetTransform(deltaX), GetTransform(deltaY));
        _screenInvalidator.InvalidatePane(1);

        var newX = MarkerPosition.X + deltaX;
        var newY = MarkerPosition.Y + deltaY;

        if (newX < 0 || newY < 0 || newX >= 8 || newY >= 8)
        {
            return;
        }

        MarkerPosition = (newX, newY);
    }

    private int GetTransform(int delta)
    {
        return _settings.RotateBoard
            ? -delta
            : delta;
    }
}

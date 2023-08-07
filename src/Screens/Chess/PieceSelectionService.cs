namespace Paraclete.Screens.Chess;

public class PieceSelectionService
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly Settings.ChessSettings _settings;

    public PieceSelectionService(ScreenInvalidator screenInvalidator, Settings settings)
    {
        _screenInvalidator = screenInvalidator;
        _settings = settings.Chess;
    }

    public (int x, int y) MarkerPosition { get; private set; }

    public void MoveSelection(int deltaX, int deltaY)
    {
        (deltaX, deltaY) = (GetTransform(deltaX), GetTransform(deltaY));
        _screenInvalidator.InvalidatePane(1);

        var newX = MarkerPosition.x + deltaX;
        var newY = MarkerPosition.y + deltaY;

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

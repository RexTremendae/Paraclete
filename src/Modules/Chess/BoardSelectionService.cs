namespace Paraclete.Modules.Chess;

public class BoardSelectionService
{
    private readonly bool _rotateBoard = false;

    public (int X, int Y) FromMarkerPosition { get; private set; }
    public (int X, int Y) ToMarkerPosition { get; private set; }

    public void MoveFromMarker((int X, int Y) delta)
    {
        delta = GetTransform(delta);

        int x = Math.Max(0, FromMarkerPosition.X + delta.X);
        x = Math.Min(x, 7);

        int y = Math.Max(0, FromMarkerPosition.Y + delta.Y);
        y = Math.Min(y, 7);

        FromMarkerPosition = (x, y);
    }

    public void SetFromMarkerPosition((int X, int Y) position)
    {
        FromMarkerPosition = position;
    }

    public void MoveToMarker((int X, int Y) delta)
    {
        delta = GetTransform(delta);

        int x = Math.Max(0, ToMarkerPosition.X + delta.X);
        x = Math.Min(x, 7);

        int y = Math.Max(0, ToMarkerPosition.Y + delta.Y);
        y = Math.Min(y, 7);

        ToMarkerPosition = (x, y);
    }

    public void SetToMarkerPosition((int X, int Y) position)
    {
        ToMarkerPosition = position;
    }

    private (int X, int Y) GetTransform((int X, int Y) pos)
    {
        return _rotateBoard
            ? (-pos.X, -pos.Y)
            : pos;
    }
}
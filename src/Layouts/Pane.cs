namespace Paraclete.Layouts;

public class Pane(int paneId, int columnIndex, (int X, int Y) position, (int X, int Y) size, bool isVisible)
{
    public static readonly Pane Empty = new(-1, -1, (0, 0), (0, 0), false);

    public bool IsVisible { get; } = isVisible;
    public (int X, int Y) Position { get; } = position;
    public (int X, int Y) Size { get; } = size;
    public int PaneId { get; } = paneId;
    public int ColumnIndex { get; } = columnIndex;

    public override string ToString()
    {
        return $"[{PaneId}] col {ColumnIndex} {Position} - {Size} [{(IsVisible ? "VISIBLE" : "NOT visible")}]";
    }
}

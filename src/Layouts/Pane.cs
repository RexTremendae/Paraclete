namespace Paraclete.Layouts;

public class Pane(int paneIndex, (int X, int Y) position, (int X, int Y) size, bool isVisible)
{
    public static readonly Pane Empty = new(-1, (0, 0), (0, 0), false);

    public bool IsVisible { get; } = isVisible;
    public (int X, int Y) Position { get; } = position;
    public (int X, int Y) Size { get; } = size;
    public int PaneIndex { get; } = paneIndex;

    public override string ToString()
    {
        return $"[{PaneIndex}] {Position} - {Size} [{(IsVisible ? "VISIBLE" : "NOT visible")}]";
    }
}

namespace Paraclete.Layouts;

public class Pane(int paneIndex, (int x, int y) position, (int x, int y) size, bool isVisible)
{
    public static readonly Pane Empty = new(-1, (0, 0), (0, 0), false);

    public bool IsVisible { get; } = isVisible;
    public (int x, int y) Position { get; } = position;
    public (int x, int y) Size { get; } = size;
    public int PaneIndex { get; } = paneIndex;

    public override string ToString()
    {
        return $"[{PaneIndex}] {Position} - {Size} [{(IsVisible ? "VISIBLE" : "NOT visible")}]";
    }
}

namespace Paraclete.Layouts;

public class Pane
{
    public static readonly Pane Empty = new Pane((0, 0), (0, 0), false);

    public Pane((int x, int y) position, (int x, int y) size, bool isVisible)
    {
        Position = position;
        Size = size;
        IsVisible = isVisible;
    }

    public bool IsVisible { get; }
    public (int x, int y) Position { get; }
    public (int x, int y) Size { get; }

    public override string ToString()
    {
        return $"{Position} - {Size} [{(IsVisible ? "VISIBLE" : "NOT visible")}]";
    }
}

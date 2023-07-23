namespace Paraclete;

public class ScreenInvalidator
{
    public bool IsAllInvalid { get; private set; } = true;
    public HashSet<int> InvalidPaneIndices { get; } = new ();

    public void InvalidateAll()
    {
        IsAllInvalid = true;
    }

    public void InvalidatePane(int index)
    {
        InvalidPaneIndices.Add(index);
    }

    public void Reset()
    {
        IsAllInvalid = false;
        InvalidPaneIndices.Clear();
    }
}

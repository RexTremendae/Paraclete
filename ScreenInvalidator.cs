namespace Paraclete;

public class ScreenInvalidator
{
    public bool IsValid { get; private set; } = true;

    public void Invalidate()
    {
        IsValid = false;
    }

    public void Reset()
    {
        IsValid = true;
    }
}

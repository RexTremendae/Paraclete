namespace Paraclete.Screens.Unicode;

public class UnicodeControl
{
    private readonly int[] _columnStartValues = new[] { 0x00c5, 0x2500, 0x2713, 0x2654, 0x2190 };
    private readonly ScreenInvalidator _screenInvalidator;

    public UnicodeControl(ScreenInvalidator screenInvalidator)
    {
        _screenInvalidator = screenInvalidator;
    }

    public int[] ColumnStartValues => _columnStartValues;
    public int SelectedColumn { get; private set; }

    public void SelectNext()
    {
        SelectedColumn++;
        if (SelectedColumn >= _columnStartValues.Length)
        {
            SelectedColumn = 0;
        }

        _screenInvalidator.InvalidatePane(0);
    }

    public void SelectPrevious()
    {
        SelectedColumn--;
        if (SelectedColumn < 0)
        {
            SelectedColumn = _columnStartValues.Length - 1;
        }

        _screenInvalidator.InvalidatePane(0);
    }

    public void SetSelectedCodepoint(int value)
    {
        _columnStartValues[SelectedColumn] = value;
        _screenInvalidator.InvalidatePane(0);
    }

    public void ScrollUp()
    {
        _columnStartValues[SelectedColumn]--;
        _screenInvalidator.InvalidatePane(0);
    }

    public void ScrollDown()
    {
        _columnStartValues[SelectedColumn]++;
        _screenInvalidator.InvalidatePane(0);
    }
}

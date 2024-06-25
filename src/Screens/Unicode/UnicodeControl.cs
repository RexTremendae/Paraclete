namespace Paraclete.Screens.Unicode;

public class UnicodeControl(ScreenInvalidator screenInvalidator)
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public int[] ColumnStartValues { get; } = [0x00c5, 0x2500, 0x2713, 0x2654, 0x2190];
    public int SelectedColumn { get; private set; }

    public void SelectNext()
    {
        SelectedColumn++;
        if (SelectedColumn >= ColumnStartValues.Length)
        {
            SelectedColumn = 0;
        }

        _screenInvalidator.InvalidatePane(UnicodeScreen.Panes.CharacterTable);
    }

    public void SelectPrevious()
    {
        SelectedColumn--;
        if (SelectedColumn < 0)
        {
            SelectedColumn = ColumnStartValues.Length - 1;
        }

        _screenInvalidator.InvalidatePane(UnicodeScreen.Panes.CharacterTable);
    }

    public void SetSelectedCodepoint(int value)
    {
        ColumnStartValues[SelectedColumn] = value;
        _screenInvalidator.InvalidatePane(UnicodeScreen.Panes.CharacterTable);
    }

    public void ScrollUp()
    {
        ColumnStartValues[SelectedColumn]--;
        _screenInvalidator.InvalidatePane(UnicodeScreen.Panes.CharacterTable);
    }

    public void ScrollDown()
    {
        ColumnStartValues[SelectedColumn]++;
        _screenInvalidator.InvalidatePane(UnicodeScreen.Panes.CharacterTable);
    }
}

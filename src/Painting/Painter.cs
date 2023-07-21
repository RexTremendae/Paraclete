namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.IO;
using Paraclete.Screens;

using static System.Console;

public class Painter
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly ScreenSelector _screenSelector;
    private readonly MenuPainter _menuPainter;
    private readonly DataInputter _dataInputter;
    private readonly DataInputPainter _dataInputPainter;

    private int _windowHeight;
    private int _windowWidth;

    public Painter(ScreenSelector screenSelector, ScreenInvalidator screenInvalidator, MenuPainter menuPainter, DataInputter dataInputter, DataInputPainter dataInputPainter)
    {
        _screenInvalidator = screenInvalidator;
        _screenSelector = screenSelector;
        _menuPainter = menuPainter;
        _dataInputter = dataInputter;
        _dataInputPainter = dataInputPainter;
    }

    public void PaintScreen(bool shortcutsMenuActive)
    {
        if (!_screenInvalidator.IsValid || _windowHeight != WindowHeight || _windowWidth != WindowWidth)
        {
            Write(AnsiSequences.ClearScreen);
            CursorVisible = false;

            _windowHeight = WindowHeight;
            _windowWidth = WindowWidth;
            _screenSelector.SelectedScreen.Layout.Recalculate(_windowWidth, _windowHeight);
            _screenSelector.SelectedScreen.Layout.Paint(this);
            _screenInvalidator.Reset();
        }

        _screenSelector.SelectedScreen.PaintContent(this, _windowWidth, _windowHeight);
        if (_dataInputter.IsActive)
        {
            _dataInputPainter.PaintInput(this, _windowWidth, _windowHeight);
        }
        else
        {
            _menuPainter.PaintMenu(this, shortcutsMenuActive, _windowWidth);
        }
    }

    public void Paint(AnsiString row, (int x, int y)? position = null)
    {
        PaintRows(new[] { row }, position);
    }

    public void PaintRows(IEnumerable<AnsiString> rows, (int x, int y)? position = null, (int x, int y)? boundary = null)
    {
        var pos = position ?? (0, 0);
        var bound = boundary ?? (_windowWidth, _windowHeight);

        if (pos.x < 0)
        {
            pos = (pos.x + _windowWidth, pos.y);
        }

        if (pos.y < 0)
        {
            pos = (pos.x, pos.y + _windowHeight);
        }

        if (bound.x < 0)
        {
            bound = (bound.x + _windowWidth, bound.y);
        }

        if (bound.y < 0)
        {
            bound = (bound.x, bound.y + _windowHeight);
        }

        var rowArray = rows.ToArray();

        try
        {
            for (int y = 0; y < rowArray.Length; y++)
            {
                WriteBounded(AnsiSequences.Reset + rowArray[y], (pos.x, pos.y + y), bound);
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error when painting.", ex);
        }
    }

    private void WriteBounded(AnsiString ansiString, (int x, int y) pos, (int x, int y) bound)
    {
        if (pos.y >= bound.y)
        {
            return;
        }

        CursorTop = pos.y;

        if (pos.x >= bound.x)
        {
            return;
        }

        CursorLeft = pos.x;

        Write(ansiString.Truncate(bound.x - pos.x));
    }
}

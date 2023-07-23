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
    private readonly TimeWriter _currentTimeWriter;

    private IScreen _selectedScreen;
    private HashSet<int> _autoRefreshingPaneIndices;
    private int _windowHeight;
    private int _windowWidth;

    public Painter(ScreenSelector screenSelector, ScreenInvalidator screenInvalidator, MenuPainter menuPainter, DataInputter dataInputter, DataInputPainter dataInputPainter)
    {
        _screenInvalidator = screenInvalidator;
        _screenSelector = screenSelector;
        _menuPainter = menuPainter;
        _dataInputter = dataInputter;
        _dataInputPainter = dataInputPainter;
        _selectedScreen = IScreen.NoScreen;
        _autoRefreshingPaneIndices = new ();

        _currentTimeWriter = new TimeWriter(new ()
        {
            FontSize = Font.Size.XS,
            Color = AnsiSequences.ForegroundColors.White,
            ShowSeconds = true,
            ShowMilliseconds = false,
        });
    }

    public void PaintScreen(bool shortcutsMenuActive)
    {
        var invalidPaneIndices = _screenInvalidator.InvalidPaneIndices.AsEnumerable();

        if (_selectedScreen != _screenSelector.SelectedScreen || _screenInvalidator.IsAllInvalid || _windowHeight != WindowHeight || _windowWidth != WindowWidth)
        {
            Write(AnsiSequences.ClearScreen);
            Write(AnsiSequences.HideCursor);

            _selectedScreen = _screenSelector.SelectedScreen;
            var layout = _selectedScreen.Layout;
            _autoRefreshingPaneIndices = _selectedScreen.AutoRefreshingPaneIndices.ToHashSet();

            _windowHeight = WindowHeight;
            _windowWidth = WindowWidth;
            layout.Recalculate(_windowWidth, _windowHeight);
            layout.Paint(this);
            invalidPaneIndices = Enumerable.Range(0, layout.Panes.Length);
        }

        foreach (var paneIdx in invalidPaneIndices.Union(_autoRefreshingPaneIndices))
        {
            _selectedScreen.GetPaintPaneAction(this, paneIdx)();
        }

        if (_dataInputter.IsActive)
        {
            _dataInputPainter.PaintInput(this, _windowWidth, _windowHeight);
        }
        else
        {
            _menuPainter.PaintMenu(this, shortcutsMenuActive, _windowWidth);
        }

        if (_selectedScreen.ShowCurrentTime)
        {
            _currentTimeWriter.Write(DateTime.Now, (-10, 1), this);
        }

        _screenInvalidator.Reset();
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

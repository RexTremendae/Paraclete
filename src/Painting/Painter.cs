namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.IO;
using Paraclete.Layouts;
using Paraclete.Screens;

public class Painter
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly ScreenSelector _screenSelector;
    private readonly MenuPainter _menuPainter;
    private readonly DataInputter _dataInputter;
    private readonly DataInputPainter _dataInputPainter;
    private readonly TimeFormatter _currentTimeFormatter;

    private IScreen _selectedScreen;
    private HashSet<int> _autoRefreshingPaneIndices;
    private int _windowHeight;
    private int _windowWidth;

    public Painter(
        ScreenSelector screenSelector,
        ScreenInvalidator screenInvalidator,
        MenuPainter menuPainter,
        DataInputter dataInputter,
        DataInputPainter dataInputPainter)
    {
        _screenInvalidator = screenInvalidator;
        _screenSelector = screenSelector;
        _menuPainter = menuPainter;
        _dataInputter = dataInputter;
        _dataInputPainter = dataInputPainter;
        _selectedScreen = IScreen.NoScreen;
        _autoRefreshingPaneIndices = [];

        _currentTimeFormatter = new (new ()
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

        if (
            _selectedScreen != _screenSelector.SelectedScreen ||
            _screenInvalidator.AreAllInvalid ||
            _windowHeight != Console.WindowHeight ||
            _windowWidth != Console.WindowWidth)
        {
            Write(AnsiSequences.ClearScreen);
            Write(AnsiSequences.EraseScrollbackBuffer);
            Write(AnsiSequences.HideCursor);

            _selectedScreen = _screenSelector.SelectedScreen;
            var layout = _selectedScreen.Layout;
            _autoRefreshingPaneIndices = [.. _selectedScreen.AutoRefreshingPaneIndices];

            _windowHeight = Console.WindowHeight;
            _windowWidth = Console.WindowWidth;
            layout.Recalculate(_windowWidth, _windowHeight);
            layout.Paint(this);
            invalidPaneIndices = Enumerable.Range(0, layout.Panes.Length);
        }

        foreach (var paneIdx in invalidPaneIndices.Union(_autoRefreshingPaneIndices))
        {
            _selectedScreen.GetPaintPaneAction(this, paneIdx)();
        }

        var cursorPos = (x: 0, y: 0);
        if (_dataInputter.IsActive)
        {
            cursorPos = _dataInputPainter.PaintInput(this, _windowWidth, _windowHeight);
        }
        else
        {
            _menuPainter.PaintMenu(this, shortcutsMenuActive, _windowWidth);
        }

        if (_selectedScreen.ShowCurrentTime)
        {
            var timeString = " " + _currentTimeFormatter.Format(DateTime.Now).First() + " ";

            PaintRows(
                new[]
                {
                    $"╤{string.Empty.PadRight(timeString.Length, '═')}╗",
                    "│" + timeString + AnsiSequences.Reset + "║",
                    $"╰{string.Empty.PadRight(timeString.Length, '─')}╢",
                },
                (-timeString.Length - 2, 0));
        }

        if (_selectedScreen.ShowTitle)
        {
            var titleString = $"  {_selectedScreen.Name}  ";

            PaintRows(
                new[]
                {
                    $"╤{string.Empty.PadRight(titleString.Length, '═')}╤",
                    "│" + AnsiSequences.ForegroundColors.Black + AnsiSequences.BackgroundColors.Blue + titleString + AnsiSequences.Reset + "│",
                    $"╰{string.Empty.PadRight(titleString.Length, '─')}╯",
                },
                (((_windowWidth - titleString.Length) / 2) - 1, 0));
        }

        if (_dataInputter.IsActive)
        {
            Write(AnsiSequences.ShowCursor);
            var x = int.Min(cursorPos.x, Console.WindowWidth - 1);
            SetCursorPosition(x, cursorPos.y);
        }
        else
        {
            Write(AnsiSequences.HideCursor);
        }
    }

    public void Paint(AnsiString row, (int x, int y)? position = null)
    {
        PaintRows(new[] { row }, position);
    }

    public void Paint(AnsiString row, Pane pane, (int x, int y)? position = null)
    {
        PaintRows(new[] { row }, pane, position);
    }

    public void PaintRows(IEnumerable<AnsiString> rows, Pane pane, (int x, int y)? position = null, bool showEllipsis = false)
    {
        var pos = ((position?.x ?? 0) + pane.Position.x, (position?.y ?? 0) + pane.Position.y);
        var boundary = (pane.Position.x + pane.Size.x, pane.Position.y + pane.Size.y);

        PaintRows(rows, pos, boundary, showEllipsis);
    }

    public void PaintRows(IEnumerable<AnsiString> rows, (int x, int y)? position = null, (int x, int y)? boundary = null, bool showEllipsis = false)
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
            0.To(rowArray.Length).Foreach(y =>
            {
                WriteBounded(AnsiSequences.Reset + rowArray[y], (pos.x, pos.y + y), bound, showEllipsis);
            });
        }
        catch (Exception ex)
        {
            Log.Error("Error when painting.", ex);
        }
    }

    private static void SetCursorPosition(int x, int y)
    {
        if (x >= Console.WindowWidth || y >= Console.WindowHeight)
        {
            return;
        }

        Console.SetCursorPosition(x, y);
    }

    private static void Write(object data)
    {
        Console.Write(data);
    }

    private static void WriteBounded(AnsiString ansiString, (int x, int y) pos, (int x, int y) bound, bool showEllipsis = false)
    {
        if (pos.y >= bound.y)
        {
            return;
        }

        if (pos.x >= bound.x)
        {
            return;
        }

        SetCursorPosition(pos.x, pos.y);

        var truncatedWidth = bound.x - pos.x;

        if (showEllipsis)
        {
            Write(ansiString.Length > truncatedWidth
                ? ansiString.Truncate(truncatedWidth - 3) + AnsiSequences.Reset + AnsiSequences.Bold + AnsiSequences.ForegroundColors.DarkYellow + "(…)"
                : ansiString);
        }
        else
        {
            Write(ansiString.Truncate(truncatedWidth));
        }
    }
}

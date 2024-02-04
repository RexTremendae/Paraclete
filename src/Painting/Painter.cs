namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.IO;
using Paraclete.Layouts;
using Paraclete.Screens;

public class Painter(
        ScreenSelector screenSelector,
        ScreenInvalidator screenInvalidator,
        MenuPainter menuPainter,
        DataInputter dataInputter,
        DataInputPainter dataInputPainter,
        BusyIndicator busyIndicator)
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
    private readonly ScreenSelector _screenSelector = screenSelector;
    private readonly MenuPainter _menuPainter = menuPainter;
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly DataInputPainter _dataInputPainter = dataInputPainter;
    private readonly BusyIndicator _busyIndicator = busyIndicator;

    private readonly TimeFormatter _currentTimeFormatter = new(new()
        {
            FontSize = Font.Size.XS,
            Color = AnsiSequences.ForegroundColors.White,
            ShowSeconds = true,
            ShowMilliseconds = false,
        });

    private IScreen _selectedScreen = IScreen.NoScreen;
    private HashSet<int> _autoRefreshingPaneIndices = [];
    private int _windowHeight;
    private int _windowWidth;

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

        var cursorPos = (X: 0, Y: 0);
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
            var x = int.Min(cursorPos.X, Console.WindowWidth - 1);
            SetCursorPosition(x, cursorPos.Y);
        }
        else
        {
            Write(AnsiSequences.HideCursor);
        }
    }

    public void Paint(AnsiString row, (int X, int Y)? position = null)
    {
        PaintRows(new[] { row }, position);
    }

    public void Paint(AnsiString row, Pane pane, (int X, int Y)? position = null)
    {
        PaintRows(new[] { row }, pane, position);
    }

    public void PaintRows(IEnumerable<AnsiString> rows, Pane pane, (int X, int Y)? position = null, bool showEllipsis = false, bool padPaneWidth = false)
    {
        var (relativeXPosition, relativeYPosition) = position ?? (0, 0);
        var absolutePosistion = (relativeXPosition + pane.Position.X, relativeYPosition + pane.Position.Y);
        var boundary = (pane.Position.X + pane.Size.X, pane.Position.Y + pane.Size.Y);

        var (isBusy, busyText) = _busyIndicator.IsPaneBusy(_selectedScreen.GetType(), pane.PaneId);

        if (isBusy)
        {
            absolutePosistion = pane.Position;
            rows = 0.To(pane.Size.Y)
                .Select(y => y == 1
                    ? new AnsiString(" ") + busyText
                    : AnsiString.Empty);
            padPaneWidth = true;
        }

        if (padPaneWidth)
        {
            rows = rows.Select(_ => _.PadRight(pane.Size.X - relativeXPosition));
        }

        PaintRows(rows, absolutePosistion, boundary, showEllipsis);
    }

    public void PaintRows(IEnumerable<AnsiString> rows, (int X, int Y)? position = null, (int X, int Y)? boundary = null, bool showEllipsis = false)
    {
        var pos = position ?? (0, 0);
        var bound = boundary ?? (_windowWidth, _windowHeight);

        if (pos.X < 0)
        {
            pos = (pos.X + _windowWidth, pos.Y);
        }

        if (pos.Y < 0)
        {
            pos = (pos.X, pos.Y + _windowHeight);
        }

        if (bound.X < 0)
        {
            bound = (bound.X + _windowWidth, bound.Y);
        }

        if (bound.Y < 0)
        {
            bound = (bound.X, bound.Y + _windowHeight);
        }

        var rowArray = rows.ToArray();

        try
        {
            0.To(rowArray.Length).Foreach(y =>
            {
                WriteBounded(AnsiSequences.Reset + rowArray[y], (pos.X, pos.Y + y), bound, showEllipsis);
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

    private static void WriteBounded(AnsiString ansiString, (int X, int Y) pos, (int X, int Y) bound, bool showEllipsis = false)
    {
        if (pos.Y >= bound.Y)
        {
            return;
        }

        if (pos.X >= bound.X)
        {
            return;
        }

        SetCursorPosition(pos.X, pos.Y);

        var truncatedWidth = bound.X - pos.X;

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

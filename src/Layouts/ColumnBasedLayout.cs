namespace Paraclete.Layouts;

using System.Text;
using Paraclete.Ansi;
using Paraclete.Painting;

public class ColumnBasedLayout : ILayout
{
    private readonly ColumnDefinition[] _columns;

    private int[] _calculatedColumnWidths = [];
    private int _windowHeight;
    private int _windowWidth;

    public ColumnBasedLayout(params ColumnDefinition[] columns)
    {
        if (columns.Count(_ => _.Width == 0) > 1)
        {
            throw new ArgumentException("Only one column can have 0 width (which means fill remaining space)", paramName: nameof(columns));
        }

        _columns = columns;
    }

    public ColumnBasedLayout(IEnumerable<ColumnDefinition> columns)
        : this(columns.ToArray())
    {
    }

    public Pane[] Panes { get; private set; } = [];

    public void Paint(Painter painter)
    {
        var frameRows = new AnsiString[_windowHeight];
        frameRows[0] = GenerateStaticRow(leftBorder: '╔', rightBorder: '╗', colBorder: '╤', padding: '═');

        1.To(_windowHeight - 4).Foreach(y =>
        {
            frameRows[y] = GenerateDynamicRow(y, _windowWidth);
        });

        // Bottom menu
        frameRows[_windowHeight - 4] = GenerateStaticRow(leftBorder: '╟', rightBorder: '╢', colBorder: '┴', padding: '─');
        frameRows[_windowHeight - 3] = GenerateStaticRow(leftBorder: '║', rightBorder: '║', colBorder: ' ', padding: ' ');
        frameRows[_windowHeight - 2] = GenerateStaticRow(leftBorder: '║', rightBorder: '║', colBorder: ' ', padding: ' ');
        frameRows[_windowHeight - 1] = GenerateStaticRow(leftBorder: '╚', rightBorder: '╝', colBorder: '═', padding: '═');

        painter.PaintRows(frameRows);
    }

    public void Recalculate(int windowWidth, int windowHeight)
    {
        _windowHeight = windowHeight;
        _windowWidth = windowWidth;

        var drawableWindowHeight = windowHeight - 4;
        var drawableWindowWidth = windowWidth - 1;

        var panes = new List<Pane>();
        var xPos = 1;
        int paneHeight;
        int paneWidth;

        _calculatedColumnWidths = new int[_columns.Length];
        var fillPaneIndex = -1;
        var totalWidth = 0;

        0.To(_columns.Length).Foreach(x =>
        {
            var columnWidth = _columns[x].Width;
            if (columnWidth == 0)
            {
                fillPaneIndex = x;
            }
            else
            {
                _calculatedColumnWidths[x] = columnWidth;
                totalWidth += columnWidth;
            }
        });

        if (fillPaneIndex >= 0)
        {
            _calculatedColumnWidths[fillPaneIndex] = (drawableWindowWidth - totalWidth - _columns.Length).ZeroFloor();
        }

        0.To(_columns.Length).Foreach(x =>
        {
            var column = _columns[x];
            paneWidth = int.Min(_calculatedColumnWidths[x], drawableWindowWidth - xPos).ZeroFloor();

            var generatedPanes = GenerateColumnPanes(
                column,
                panes.Count,
                xPos: xPos,
                paneWidth: paneWidth,
                drawableWindowHeight: drawableWindowHeight);

            panes.AddRange(generatedPanes);
            xPos += _calculatedColumnWidths[x] + 1;
        });

        paneWidth = (drawableWindowWidth - xPos).ZeroFloor();
        paneHeight = (drawableWindowHeight - 1).ZeroFloor();

        if (paneWidth > 0 && paneHeight > 0)
        {
            panes.Add(new(panes.Count, (xPos, 1), (paneWidth, paneHeight), true));
        }

        Panes = [.. panes];
    }

    private static IEnumerable<Pane> GenerateColumnPanes(ColumnDefinition column, int firstPaneIndex, int xPos, int paneWidth, int drawableWindowHeight)
    {
        var yPos = 1;
        var paneHeight = 0;

        var panes = new List<Pane>();
        var paneIndex = firstPaneIndex;
        0.To(column.CellHeights.Length).Foreach(y =>
        {
            paneHeight = int.Min(column.CellHeights[y], drawableWindowHeight - yPos).ZeroFloor();

            if (paneHeight > 0 && paneWidth > 0)
            {
                panes.Add(new(paneIndex, (xPos, yPos), (paneWidth, paneHeight), true));
            }
            else
            {
                panes.Add(new(paneIndex, (xPos, yPos), (0, 0), false));
            }

            yPos += paneHeight + 1;
            paneIndex++;
        });

        paneHeight = (drawableWindowHeight - yPos).ZeroFloor();

        if (paneHeight > 0 && paneWidth > 0)
        {
            panes.Add(new(paneIndex, (xPos, yPos), (paneWidth, paneHeight), true));
        }
        else
        {
            panes.Add(new(paneIndex, (xPos, yPos), (0, 0), false));
        }

        return panes;
    }

    private static bool IsDivider(int rowIndex, ColumnDefinition col)
    {
        var totalHeight = 1;
        foreach (var height in col.CellHeights)
        {
            totalHeight += height;
            if (rowIndex < totalHeight)
            {
                return false;
            }
            else if (rowIndex == totalHeight)
            {
                return true;
            }

            totalHeight++;
        }

        return false;
    }

    private string GenerateDynamicRow(int rowIndex, int windowWidth)
    {
        var rowBuilder = new StringBuilder();
        var totalWidth = 0;

        var colIsDivider = false;
        var isFirst = true;
        var lastColIsDivider = false;
        bool prevColIsDivider;

        foreach (var x in 0.To(_calculatedColumnWidths.Length))
        {
            var colWidth = _calculatedColumnWidths[x];
            prevColIsDivider = colIsDivider;
            colIsDivider = IsDivider(rowIndex, _columns[x]);
            lastColIsDivider = colIsDivider;
            var colDividerChar = 0 switch
            {
                var _ when isFirst => colIsDivider ? '╟' : '║',
                var _ when prevColIsDivider && colIsDivider  => '┼',
                var _ when !prevColIsDivider && colIsDivider => '├',
                var _ when prevColIsDivider && !colIsDivider => '┤',
                _                                            => '│',
            };
            isFirst = false;

            rowBuilder.Append(colDividerChar);
            rowBuilder.Append(string.Empty.PadRight(colWidth, colIsDivider ? '─' : ' '));
            totalWidth += colWidth + 1;

            if (totalWidth > _windowWidth - 2)
            {
                break;
            }
        }

        rowBuilder.Append(colIsDivider ? '┤' : '│');
        totalWidth++;

        var truncateWidth = _windowWidth - 1;
        if (rowBuilder.Length > truncateWidth)
        {
            rowBuilder.Remove(truncateWidth, rowBuilder.Length - truncateWidth);
            totalWidth = truncateWidth;
        }

        rowBuilder.Append(string.Empty.PadRight(int.Max(windowWidth - totalWidth - 1, 0)));
        rowBuilder.Append(lastColIsDivider ? '╢' : '║');

        return rowBuilder.ToString();
    }

    private string GenerateStaticRow(char leftBorder, char rightBorder, char colBorder, char padding)
    {
        var rowBuilder = new StringBuilder();
        rowBuilder.Append(leftBorder);
        var totalWidth = 1;

        foreach (var colWidth in _calculatedColumnWidths)
        {
            rowBuilder.Append(string.Empty.PadRight(colWidth, padding));
            rowBuilder.Append(colBorder);
            totalWidth += colWidth + 1;
        }

        var truncateWidth = _windowWidth - 1;
        if (rowBuilder.Length > truncateWidth)
        {
            rowBuilder.Remove(truncateWidth, rowBuilder.Length - truncateWidth);
        }

        rowBuilder.Append(string.Empty.PadRight(int.Max(_windowWidth - totalWidth - 1, 0), padding));
        rowBuilder.Append(rightBorder);

        return rowBuilder.ToString();
    }

    public class ColumnDefinition(int width, params int[] cellHeights)
    {
        public int Width { get; } = width;
        public int[] CellHeights { get; } = cellHeights;
    }
}

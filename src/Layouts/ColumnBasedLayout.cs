namespace Paraclete.Layouts;

using System.Text;
using Paraclete.Ansi;
using Paraclete.Painting;

public class ColumnBasedLayout : ILayout
{
    private readonly ColumnDefinition[] _columns;

    private int[] _calculatedColumnWidths = [];
    private int[][] _calculatedPaneHeights = [[]];

    private int _windowHeight;
    private int _windowWidth;

    public ColumnBasedLayout(params ColumnDefinition[] columns)
    {
        if (columns.Count(_ => _.Width == 0) > 1)
        {
            throw new ArgumentException("Only one column can have 0 width (which means fill remaining space)", paramName: nameof(columns));
        }

        if (columns.Any(c => c.CellHeights.Count(_ => _ == 0) > 1))
        {
            throw new ArgumentException("Only one pane per column can have 0 height (which means fill remaining space)", paramName: nameof(columns));
        }

        _columns = columns;
    }

    public ColumnBasedLayout(IEnumerable<ColumnDefinition> columns)
        : this([..columns])
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
        _calculatedPaneHeights = new int[_columns.Length][];
        var fillColumnIndex = -1;
        var fillPaneIndices = new int[_columns.Length];
        0.To(_columns.Length).Foreach(idx =>
        {
            fillPaneIndices[idx] = -1;
        });

        var totalWidth = 0;
        var totalHeights = new int[_columns.Length];

        0.To(_columns.Length).Foreach(x =>
        {
            var column = _columns[x];
            if (column.Width == 0)
            {
                fillColumnIndex = x;
            }
            else
            {
                _calculatedColumnWidths[x] = column.Width;
                totalWidth += column.Width;
            }

            _calculatedPaneHeights[x] = new int[column.CellHeights.Length];
            totalHeights[x] = 0;
            foreach (var y in 0.To(column.CellHeights.Length))
            {
                var cellHeight = column.CellHeights[y];
                _calculatedPaneHeights[x][y] = cellHeight;
                totalHeights[x] += cellHeight;

                if (cellHeight == 0)
                {
                    fillPaneIndices[x] = y;
                }
            }
        });

        if (fillColumnIndex >= 0)
        {
            _calculatedColumnWidths[fillColumnIndex] = (drawableWindowWidth - totalWidth - _columns.Length).ZeroFloor();
        }

        0.To(_columns.Length).Foreach(x =>
        {
            if (fillPaneIndices[x] >= 0)
            {
                _calculatedPaneHeights[x][fillPaneIndices[x]] =
                    (drawableWindowHeight - totalHeights[x] - _calculatedPaneHeights[x].Length)
                    .ZeroFloor();
            }

            paneWidth = int.Min(_calculatedColumnWidths[x], drawableWindowWidth - xPos).ZeroFloor();

            var generatedPanes = GenerateColumnPanes(
                x,
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
            panes.Add(new(
                paneId: panes.Count,
                columnIndex: _columns.Length,
                position: (xPos, 1),
                size: (paneWidth, paneHeight),
                isVisible: true));
        }

        Panes = [.. panes];
    }

    private IEnumerable<Pane> GenerateColumnPanes(int columnIdx, int firstPaneId, int xPos, int paneWidth, int drawableWindowHeight)
    {
        var yPos = 1;
        int paneHeight;

        var panes = new List<Pane>();
        var paneId = firstPaneId;
        foreach (var y in 0.To(_calculatedPaneHeights[columnIdx].Length))
        {
            paneHeight = int.Min(_calculatedPaneHeights[columnIdx][y], drawableWindowHeight - yPos).ZeroFloor();

            if (paneHeight > 0 && paneWidth > 0)
            {
                panes.Add(new(
                    paneId: paneId,
                    columnIndex: columnIdx,
                    position: (xPos, yPos),
                    size: (paneWidth, paneHeight),
                    isVisible: true));
            }
            else
            {
                panes.Add(new(
                    paneId: paneId,
                    columnIndex: columnIdx,
                    position: (xPos, yPos),
                    size: (0, 0),
                    isVisible: false));
            }

            yPos += paneHeight + 1;
            paneId++;
        }

        paneHeight = (drawableWindowHeight - yPos).ZeroFloor();

        if (paneHeight > 0 && paneWidth > 0)
        {
            panes.Add(new(
                paneId: paneId,
                columnIndex: columnIdx,
                position: (xPos, yPos),
                size: (paneWidth, paneHeight),
                isVisible: true));
        }

        return panes;
    }

    private bool IsDivider(int rowIndex, int colIdx)
    {
        var totalHeight = 1;
        foreach (var height in _calculatedPaneHeights[colIdx])
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
            colIsDivider = IsDivider(rowIndex, x);
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

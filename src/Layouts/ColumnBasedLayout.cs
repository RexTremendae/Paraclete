namespace Paraclete.Layouts;

using System.Text;
using Paraclete.Ansi;
using Paraclete.Painting;

public class ColumnBasedLayout(params ColumnBasedLayout.ColumnDefinition[] columns) : ILayout
{
    private readonly ColumnDefinition[] _columns = columns;

    private int _windowHeight;
    private int _windowWidth;

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

        0.To(_columns.Length).Foreach(x =>
        {
            var column = _columns[x];
            paneWidth = int.Min(column.Width, drawableWindowWidth - xPos).ZeroFloor();
            panes.AddRange(GenerateColumnPanes(column, panes.Count, xPos: xPos, paneWidth: paneWidth, drawableWindowHeight: drawableWindowHeight));
            xPos += column.Width + 1;
        });

        paneWidth = (drawableWindowWidth - xPos).ZeroFloor();
        paneHeight = (drawableWindowHeight - 1).ZeroFloor();

        if (paneWidth > 0 && paneHeight > 0)
        {
            panes.Add(new(panes.Count, (xPos, 1), (paneWidth, paneHeight), true));
        }
        else
        {
            panes.Add(new(panes.Count, (xPos, 1), (0, 0), false));
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
        bool lastColIsDivider;

        foreach (var col in _columns)
        {
            lastColIsDivider = colIsDivider;
            colIsDivider = IsDivider(rowIndex, col);
            var colDividerChar = 0 switch
            {
                var _ when isFirst => colIsDivider ? '╟' : '║',
                var _ when lastColIsDivider && colIsDivider  => '┼',
                var _ when !lastColIsDivider && colIsDivider => '├',
                var _ when lastColIsDivider && !colIsDivider => '┤',
                _                                            => '│',
            };

            rowBuilder.Append(colDividerChar);
            rowBuilder.Append(string.Empty.PadRight(col.Width, colIsDivider ? '─' : ' '));
            totalWidth += col.Width + 1;
            isFirst = false;
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
        rowBuilder.Append('║');

        return rowBuilder.ToString();
    }

    private string GenerateStaticRow(char leftBorder, char rightBorder, char colBorder, char padding)
    {
        var rowBuilder = new StringBuilder();
        rowBuilder.Append(leftBorder);
        var totalWidth = 1;

        foreach (var colWidth in _columns.Select(_ => _.Width))
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

namespace Paraclete.Layouts;

using System.Text;
using Paraclete.Ansi;
using Paraclete.Painting;

public class ColumnBasedLayout : ILayout
{
    private readonly ColumnDefinition[] _columns;

    private int _windowHeight;
    private int _windowWidth;

    public ColumnBasedLayout(IEnumerable<ColumnDefinition> columns)
        : this(columns.ToArray())
    {
    }

    public ColumnBasedLayout(params ColumnDefinition[] columns)
    {
        _columns = columns;
        Panes = Array.Empty<Pane>();
    }

    public Pane[] Panes { get; private set; }

    public void Paint(Painter painter)
    {
        var frameRows = new AnsiString[_windowHeight];
        frameRows[0] = GenerateStaticRow(leftBorder: '╔', rightBorder: '╗', colBorder: '╤', padding: '═');

        for (int y = 1; y < _windowHeight - 4; y++)
        {
            frameRows[y] = GenerateDynamicRow(y, _windowWidth);
        }

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

        for (var x = 0; x < _columns.Length; x++)
        {
            var yPos = 1;
            var definition = _columns[x];
            paneWidth = int.Max(0, int.Min(definition.Width, drawableWindowWidth - xPos));

            for (int y = 0; y < definition.CellHeights.Length; y++)
            {
                paneHeight = int.Max(0, int.Min(definition.CellHeights[y], drawableWindowHeight - yPos));

                if (paneHeight > 0 && paneWidth > 0)
                {
                    panes.Add(new ((xPos, yPos), (paneWidth, paneHeight), true));
                }
                else
                {
                    panes.Add(new ((xPos, yPos), (0, 0), false));
                }

                yPos += paneHeight + 1;
            }

            paneHeight = int.Max(0, drawableWindowHeight - yPos);

            if (paneHeight > 0 && paneWidth > 0)
            {
                panes.Add(new ((xPos, yPos), (paneWidth, paneHeight), true));
            }
            else
            {
                panes.Add(new ((xPos, yPos), (0, 0), false));
            }

            xPos += definition.Width + 1;
        }

        paneWidth = int.Max(0, drawableWindowWidth - xPos);
        paneHeight = int.Max(0, drawableWindowHeight - 1);

        if (paneWidth > 0 && paneHeight > 0)
        {
            panes.Add(new ((xPos, 1), (paneWidth, paneHeight), true));
        }
        else
        {
            panes.Add(new ((xPos, 1), (0, 0), false));
        }

        Panes = panes.ToArray();
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

            var colDividerChar = '│';
            if (isFirst)
            {
                colDividerChar = colIsDivider ? '╟' : '║';
            }
            else if (colIsDivider && lastColIsDivider)
            {
                colDividerChar = '┼';
            }
            else if (colIsDivider && !lastColIsDivider)
            {
                colDividerChar = '├';
            }
            else if (!colIsDivider && lastColIsDivider)
            {
                colDividerChar = '┤';
            }

            colDividerChar = 0 switch
            {
                var _ when isFirst => colIsDivider ? '╟' : '║',
                var _ when lastColIsDivider && colIsDivider  => '┼',
                var _ when !lastColIsDivider && colIsDivider => '├',
                var _ when lastColIsDivider && !colIsDivider => '┤',
                _                                            => '│'
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
        rowBuilder.Append("║");

        return rowBuilder.ToString();
    }

    private bool IsDivider(int rowIndex, ColumnDefinition col)
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

    private string GenerateStaticRow(char leftBorder, char rightBorder, char colBorder, char padding)
    {
        var rowBuilder = new StringBuilder();
        rowBuilder.Append(leftBorder);
        var totalWidth = 1;

        foreach (var col in _columns)
        {
            rowBuilder.Append(string.Empty.PadRight(col.Width, padding));
            rowBuilder.Append(colBorder);
            totalWidth += col.Width + 1;
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

    public class ColumnDefinition
    {
        public ColumnDefinition(int width, params int[] cellHeights)
        {
            Width = width;
            CellHeights = cellHeights;
        }

        public int Width { get; }
        public int[] CellHeights { get; }
    }
}

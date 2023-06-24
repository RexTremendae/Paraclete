using System.Text;
using Paraclete.Painting;

namespace Paraclete.Layouts;

public class ColumnBasedLayout : ILayout
{
    public class ColumnDefinition
    {
        public int Width { get; }
        public int[] CellHeights { get; }

        public ColumnDefinition(int width, params int[] cellHeights)
        {
            Width = width;
            CellHeights = cellHeights;
        }
    }

    private ColumnDefinition[] _columns;

    public ColumnBasedLayout(params ColumnDefinition[] columns)
    {
        _columns = columns;
    }

    public void Paint(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = GenerateStaticRow(leftBorder: '╔', rightBorder: '╗', colBorder: '╤', padding: '═', windowWidth);

        for (int y = 1; y < windowHeight-4; y++)
        {
            frameRows[y] = GenerateDynamicRow(y, windowWidth);
        }

        // Bottom menu
        frameRows[windowHeight-4] = GenerateStaticRow(leftBorder: '╟', rightBorder: '╢', colBorder: '┴', padding: '─', windowWidth);
        frameRows[windowHeight-3] = GenerateStaticRow(leftBorder: '║', rightBorder: '║', colBorder: ' ', padding: ' ', windowWidth);
        frameRows[windowHeight-2] = GenerateStaticRow(leftBorder: '║', rightBorder: '║', colBorder: ' ', padding: ' ', windowWidth);
        frameRows[windowHeight-1] = GenerateStaticRow(leftBorder: '╚', rightBorder: '╝', colBorder: '═', padding: '═', windowWidth);

        painter.PaintRows(frameRows);
    }

    private string GenerateDynamicRow(int rowIndex, int windowWidth)
    {
        var rowBuilder = new StringBuilder();
        var totalWidth = 0;

        var colIsDivider = false;
        var lastColIsDivider = false;
        var isFirst = true;

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
                var x when isFirst => colIsDivider ? '╟' : '║',
                var x when lastColIsDivider && colIsDivider  => '┼',
                var x when !lastColIsDivider && colIsDivider => '├',
                var x when lastColIsDivider && !colIsDivider => '┤',
                _                                            => '│'
            };

            rowBuilder.Append(colDividerChar);
            rowBuilder.Append("".PadRight(col.Width, colIsDivider ? '─' : ' '));
            totalWidth += col.Width + 1;
            isFirst = false;
        }

        rowBuilder.Append(colIsDivider ? '┤' : '│');
        totalWidth ++;

        rowBuilder.Append("".PadRight(int.Max(windowWidth - totalWidth - 1, 0)));
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
            totalHeight ++;
        }

        return false;
    }

    private string GenerateStaticRow(char leftBorder, char rightBorder, char colBorder, char padding, int windowWidth)
    {
        var rowBuilder = new StringBuilder();
        rowBuilder.Append(leftBorder);
        var totalWidth = 1;
        foreach (var col in _columns)
        {
            rowBuilder.Append("".PadRight(col.Width, padding));
            rowBuilder.Append(colBorder);
            totalWidth += col.Width + 1;
        }
        rowBuilder.Append("".PadRight(int.Max(windowWidth - totalWidth - 1, 0), padding));
        rowBuilder.Append(rightBorder);

        return rowBuilder.ToString();
    }
}

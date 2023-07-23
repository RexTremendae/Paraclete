namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

public class ColumnLayoutExhibition : IExhibition
{
    private static readonly int[][] PaneHeights = new int[][]
    {
        new int[] { },
        new int[] { 5, 8, 12 },
        new int[] { 18 },
        new int[] { 15, 15 },
    };

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition[]
    {
        new (40, PaneHeights[0]),
        new (16, PaneHeights[1]),
        new (20, PaneHeights[2]),
        new (25, PaneHeights[3]),
    });

    public void Paint(Painter painter, (int x, int y) position, int paneIndex)
    {
        var rows = new List<AnsiString>();
        var pane = Layout.Panes[paneIndex];

        var backgroundColor = AnsiSequences.BackgroundColor(0x22, 0x33, 0x44);
        var edgeForegroundColor = AnsiSequences.ForegroundColors.Gray;

        var contentWidth = pane.Size.x - 2;

        for (int y = 0; y < pane.Size.y; y++)
        {
            var contentBuilder = new AnsiStringBuilder();

            if (y == 0)
            {
                var paddingWidthLeft = (contentWidth - 6) / 2;
                var paddingWidthRight = (int)Math.Ceiling((contentWidth - 6) / 2d);

                contentBuilder
                    .Append(edgeForegroundColor)
                    .Append(string.Empty.PadRight(paddingWidthLeft, '-'))
                    .Append(" #")
                    .Append(AnsiSequences.ForegroundColors.Blue)
                    .Append(paneIndex.ToString("00"))
                    .Append(edgeForegroundColor)
                    .Append("# ")
                    .Append(string.Empty.PadRight(paddingWidthRight, '-'))
                ;
            }
            else if (y == pane.Size.y - 1)
            {
                contentBuilder
                    .Append(edgeForegroundColor)
                    .Append(string.Empty.PadRight(contentWidth, '-'))
                ;
            }
            else if (paneIndex == 0)
            {
                var paneIdx = y - 4;

                if (paneIdx >= 0 && paneIdx < Layout.Panes.Length)
                {
                    var listedPane = Layout.Panes[paneIdx];
                    contentBuilder
                        .Append("  pane ")
                        .Append(AnsiSequences.BackgroundColors.Gray)
                        .Append(AnsiSequences.ForegroundColors.Black)
                        .Append(paneIdx.ToString("00"))
                        .Append(AnsiSequences.Reset)
                        .Append(backgroundColor)
                        .Append(": ")
                        .Append($"({listedPane.Position.x.ToString().PadLeft(3)}, {listedPane.Position.y.ToString().PadLeft(3)})")
                        .Append("  ")
                        .Append($"({listedPane.Size.x.ToString().PadLeft(3)} x {listedPane.Size.y.ToString().PadLeft(3)})")
                        .Append(string.Empty.PadRight(contentWidth - contentBuilder.Length))
                    ;

                    paneIdx++;
                }
                else
                {
                    contentBuilder.Append(string.Empty.PadRight(contentWidth));
                }
            }
            else
            {
                contentBuilder.Append(string.Empty.PadRight(contentWidth));
            }

            var data = new AnsiStringBuilder();

            data
                .Append(backgroundColor)
                .Append(edgeForegroundColor)
                .Append((y == 0 || y == pane.Size.y - 1) ? "●" : "|")
                .Append(contentBuilder)
                .Append(backgroundColor)
                .Append(edgeForegroundColor)
                .Append((y == 0 || y == pane.Size.y - 1) ? "●" : "|")
            ;

            rows.Add(data.Build());
        }

        painter.PaintRows(rows, pane.Position);
    }
}

namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

public class ColumnLayoutExhibition : IExhibition
{
    private static readonly (int width, int[] heights)[] PaneSizes = new (int, int[])[]
    {
        (50, new int[] { }),
        (16, new int[] { 5, 8, 12 }),
        (20, new int[] { 18 }),
        (25, new int[] { 15, 15 }),
    };

    public ILayout Layout { get; } = new ColumnBasedLayout(
        PaneSizes.Select(_ => new ColumnBasedLayout.ColumnDefinition(_.width, _.heights)));

    public void Paint(Painter painter, (int x, int y) position, int paneIndex)
    {
        var rows = new List<AnsiString>();
        var pane = Layout.Panes[paneIndex];

        var backgroundColor = AnsiSequences.BackgroundColor(0x22, 0x33, 0x44);
        var edgeForegroundColor = AnsiSequences.ForegroundColors.Gray;

        var contentWidth = int.Max(0, pane.Size.x - 2);

        for (int y = 0; y < pane.Size.y; y++)
        {
            var contentBuilder = new AnsiStringBuilder();

            if (y == 0)
            {
                var paddingWidthLeft = int.Max(0, (contentWidth - 6) / 2);
                var paddingWidthRight = int.Max(0, (int)Math.Ceiling((contentWidth - 6) / 2d));

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
                        .Append($"({listedPane.Size.x.ToString().PadLeft(3)} x {listedPane.Size.y.ToString().PadLeft(3)}) ")
                        .Append($"visible: ")
                        .Append(listedPane.IsVisible ? AnsiSequences.ForegroundColors.Green + "✓" : AnsiSequences.ForegroundColors.Red + "✘")
                        .Append(backgroundColor)
                        .Append(string.Empty.PadRight(contentWidth - contentBuilder.Length))
                    ;
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
            ;

            if (data.Length < pane.Size.x)
            {
                data
                    .Append(backgroundColor)
                    .Append(edgeForegroundColor)
                    .Append((y == 0 || y == pane.Size.y - 1) ? "●" : "|");
            }

            rows.Add(data.Build());
        }

        painter.PaintRows(rows, pane);
    }
}

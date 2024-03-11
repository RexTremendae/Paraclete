namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

public class ColumnLayoutExhibition : IExhibition
{
    private static readonly AnsiControlSequence _backgroundColor = AnsiSequences.BackgroundColor(0x22, 0x33, 0x44);
    private static readonly AnsiControlSequence _foregroundColor = AnsiSequences.ForegroundColors.Gray;

    private static readonly (int Width, int[] Heights)[] PaneSizes =
    [
        (50, []),
        (0,  [5, 0, 12]),
        (20, [18]),
        (25, [20, 10]),
    ];

    public ILayout Layout { get; } = new ColumnBasedLayout(
        PaneSizes.Select(_ => new ColumnBasedLayout.ColumnDefinition(_.Width, _.Heights)));

    public void Paint(Painter painter, (int X, int Y) position, int paneIndex)
    {
        var rows = new List<AnsiString>();
        var pane = Layout.Panes[paneIndex];

        rows.Add(GetTopRow(pane.PaneId.ToString("00"), pane.Size.X));
        var end = int.Max(1, pane.Size.Y - 1);
        rows.AddRange(GetMiddleRows(startRow: 1, endRow: end, paneIndex: paneIndex, paneWidth: pane.Size.X));
        rows.Add(GetBottomRow(pane.Size.X));

        painter.PaintRows(rows, pane);
    }

    private static AnsiString GetTopRow(string title, int paneWidth)
    {
        var ansiTitle = " # " + AnsiSequences.ForegroundColors.Blue + title + _foregroundColor + " # ";
        var paddingWidthLeft = (((paneWidth - ansiTitle.Length) / 2) - 1).ZeroFloor();
        var paddingWidthRight = (paneWidth - ansiTitle.Length - paddingWidthLeft).ZeroFloor();

        return new AnsiStringBuilder()
            .Append(_foregroundColor)
            .Append(_backgroundColor)
            .Append("●")
            .Append(string.Empty.PadRight(paddingWidthLeft, '-'))
            .Append(ansiTitle)
            .Append(string.Empty.PadRight(paddingWidthRight, '-'))
            .Build()
            .Truncate(paneWidth - 1) +
            _foregroundColor +
            "●";
    }

    private static AnsiString GetBottomRow(int paneWidth)
    {
        return new AnsiStringBuilder()
            .Append(_foregroundColor)
            .Append(_backgroundColor)
            .Append("●")
            .Append(string.Empty.PadRight((paneWidth - 2).ZeroFloor(), '-'))
            .Append("●")
            .Build();
    }

    private IEnumerable<AnsiString> GetMiddleRows(int startRow, int endRow, int paneIndex, int paneWidth)
    {
        var displayPaneIdx = 0;
        var displayEmptyRow = false;
        var contentWidth = (paneWidth - 2).ZeroFloor();
        var emptyRowContent = string.Empty.PadRight(contentWidth);
        var currentColumnIndex = 0;
        var paneToDisplay = Layout.Panes[0];

        foreach (var y in startRow.To(endRow))
        {
            var contentBuilder = new AnsiStringBuilder()
                .Append(_foregroundColor)
                .Append(_backgroundColor)
                .Append("|");

            if (paneIndex == 0)
            {
                if (y < 4 || displayEmptyRow || displayPaneIdx >= Layout.Panes.Length)
                {
                    displayEmptyRow = false;
                    contentBuilder.Append(emptyRowContent);
                }
                else
                {
                    var visibilityMarker = paneToDisplay.IsVisible
                        ? AnsiSequences.ForegroundColors.Green + "✓"
                        : AnsiSequences.ForegroundColors.Red + "✘";

                    contentBuilder
                        .Append("  pane ")
                        .Append(AnsiSequences.BackgroundColors.Gray)
                        .Append(AnsiSequences.ForegroundColors.Black)
                        .Append(displayPaneIdx.ToString("00"))
                        .Append(AnsiSequences.Reset)
                        .Append(_foregroundColor)
                        .Append(_backgroundColor)
                        .Append(": ")
                        .Append($"({paneToDisplay.Position.X,3}, {paneToDisplay.Position.Y,3})")
                        .Append("  ")
                        .Append($"({paneToDisplay.Size.X,3} x {paneToDisplay.Size.Y,3}) ")
                        .Append($"visible: ")
                        .Append(visibilityMarker)
                        .Append(string.Empty.PadRight(contentWidth - contentBuilder.Length + 1))
                    ;

                    displayPaneIdx++;
                    if (displayPaneIdx < Layout.Panes.Length)
                    {
                        paneToDisplay = Layout.Panes[displayPaneIdx];

                        if (currentColumnIndex != paneToDisplay.ColumnIndex)
                        {
                            displayEmptyRow = true;
                            currentColumnIndex = paneToDisplay.ColumnIndex;
                        }
                    }
                }
            }
            else
            {
                contentBuilder.Append(emptyRowContent);
            }

            contentBuilder
                .Append(_foregroundColor)
                .Append("|")
                .Build();

            yield return contentBuilder.Build();
        }
    }
}

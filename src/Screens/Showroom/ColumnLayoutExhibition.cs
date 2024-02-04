namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

public class ColumnLayoutExhibition : IExhibition
{
    private static readonly AnsiControlSequence _backgroundColor = AnsiSequences.BackgroundColor(0x22, 0x33, 0x44);
    private static readonly AnsiControlSequence _foregroundColor = AnsiSequences.ForegroundColors.Gray;

    private static readonly (int Width, int[] Heights)[] PaneSizes = new (int, int[])[]
    {
        (50, Array.Empty<int>()),
        (0,  new int[] { 5, 8, 12 }),
        (20, new int[] { 18 }),
        (25, new int[] { 15, 15 }),
    };

    public ILayout Layout { get; } = new ColumnBasedLayout(
        PaneSizes.Select(_ => new ColumnBasedLayout.ColumnDefinition(_.Width, _.Heights)));

    public void Paint(Painter painter, (int X, int Y) position, int paneIndex)
    {
        var rows = new List<AnsiString>();
        var pane = Layout.Panes[paneIndex];

        AddTopRow(rows, pane.PaneIndex.ToString("00"), pane.Size.X);
        var end = int.Max(1, pane.Size.Y - 1);
        1.To(end).Foreach(y =>
        {
            AddMiddleRow(rows, paneIndex: paneIndex, paneWidth: pane.Size.X, rowIndex: y);
        });

        AddBottomRow(rows, pane.Size.X);

        painter.PaintRows(rows, pane);
    }

    private static void AddTopRow(List<AnsiString> rows, string title, int paneWidth)
    {
        var ansiTitle = " # " + AnsiSequences.ForegroundColors.Blue + title + _foregroundColor + " # ";
        var paddingWidthLeft = (((paneWidth - ansiTitle.Length) / 2) - 1).ZeroFloor();
        var paddingWidthRight = (paneWidth - ansiTitle.Length - paddingWidthLeft).ZeroFloor();

        Log.Information("'{0}' - left: {1} - right: {2}", ansiTitle.ToString(), paddingWidthLeft, paddingWidthRight);
        Log.Information("L: {0} R: {1}", string.Empty.PadRight(paddingWidthLeft, '-'), string.Empty.PadRight(paddingWidthRight, '-'));

        rows.Add(new AnsiStringBuilder()
            .Append(_foregroundColor)
            .Append(_backgroundColor)
            .Append("●")
            .Append(string.Empty.PadRight(paddingWidthLeft, '-'))
            .Append(ansiTitle)
            .Append(string.Empty.PadRight(paddingWidthRight, '-'))
            .Build()
            .Truncate(paneWidth - 1) +
            _foregroundColor +
            "●");
    }

    private static void AddBottomRow(List<AnsiString> rows, int paneWidth)
    {
        rows.Add(new AnsiStringBuilder()
            .Append(_foregroundColor)
            .Append(_backgroundColor)
            .Append("●")
            .Append(string.Empty.PadRight((paneWidth - 2).ZeroFloor(), '-'))
            .Append("●")
            .Build());
    }

    private void AddMiddleRow(List<AnsiString> rows, int paneIndex, int paneWidth, int rowIndex)
    {
        var contentWidth = (paneWidth - 2).ZeroFloor();
        var contentBuilder = new AnsiStringBuilder()
            .Append(_foregroundColor)
            .Append(_backgroundColor)
            .Append("|");

        if (paneIndex == 0)
        {
            var paneIdx = rowIndex - 4;

            if (paneIdx >= 0 && paneIdx < Layout.Panes.Length)
            {
                var listedPane = Layout.Panes[paneIdx];
                var visibilityMarker = listedPane.IsVisible
                    ? AnsiSequences.ForegroundColors.Green + "✓"
                    : AnsiSequences.ForegroundColors.Red + "✘";

                contentBuilder
                    .Append("  pane ")
                    .Append(AnsiSequences.BackgroundColors.Gray)
                    .Append(AnsiSequences.ForegroundColors.Black)
                    .Append(paneIdx.ToString("00"))
                    .Append(AnsiSequences.Reset)
                    .Append(_foregroundColor)
                    .Append(_backgroundColor)
                    .Append(": ")
                    .Append($"({listedPane.Position.X,3}, {listedPane.Position.Y,3})")
                    .Append("  ")
                    .Append($"({listedPane.Size.X,3} x {listedPane.Size.Y,3}) ")
                    .Append($"visible: ")
                    .Append(visibilityMarker)
                    .Append(string.Empty.PadRight(contentWidth - contentBuilder.Length + 1))
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

        rows.Add(contentBuilder
            .Append(_foregroundColor)
            .Append("|")
            .Build());
    }
}

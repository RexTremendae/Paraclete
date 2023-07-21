namespace Paraclete.Screens;

using System.Text;
using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

using static Paraclete.Ansi.AnsiSequences;

public interface IExhibition
{
    ILayout Layout { get; }
    void Paint(Painter painter, (int x, int y) position);
}

public class FontExhibition : IExhibition
{
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position)
    {
        var text = "0123456789:.";

        var colors = new Dictionary<Font.Size, AnsiControlSequence>
        {
            { Font.Size.XS, ForegroundColors.DarkBlue },
            { Font.Size.S,  ForegroundColors.Blue },
            { Font.Size.M,  ForegroundColors.DarkCyan },
            { Font.Size.L,  ForegroundColors.Cyan },
        };

        foreach (var size in Enum.GetValues<Font.Size>())
        {
            if (size == Font.Size.Undefined)
            {
                continue;
            }

            var fontWriter = FontWriter.Create(size);
            painter.Paint($"{ForegroundColors.White}{size.ToString()}:".PadRight(4), position);
            fontWriter.Write(text, colors[size], (position.x + 4, position.y), painter);
            position = (position.x, position.y + fontWriter.Font.CharacterHeight + 1);
        }
    }
}

public class ColorExhibition : IExhibition
{
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position)
    {
        var initialPosition = position;

        var columnWidth = 16;
        var colorNumber = 0;

        foreach (var color in Enum.GetValues<ConsoleColor>())
        {
            var colorName = color switch {
                    ConsoleColor.Black       => ForegroundColors.Black + BackgroundColors.Gray,
                    ConsoleColor.DarkBlue    => ForegroundColors.DarkBlue,
                    ConsoleColor.DarkGreen   => ForegroundColors.DarkGreen,
                    ConsoleColor.DarkCyan    => ForegroundColors.DarkCyan,
                    ConsoleColor.DarkRed     => ForegroundColors.DarkRed,
                    ConsoleColor.DarkMagenta => ForegroundColors.DarkMagenta,
                    ConsoleColor.DarkYellow  => ForegroundColors.DarkYellow,
                    ConsoleColor.Gray        => ForegroundColors.Gray,
                    ConsoleColor.DarkGray    => ForegroundColors.DarkGray,
                    ConsoleColor.Blue        => ForegroundColors.Blue,
                    ConsoleColor.Green       => ForegroundColors.Green,
                    ConsoleColor.Cyan        => ForegroundColors.Cyan,
                    ConsoleColor.Red         => ForegroundColors.Red,
                    ConsoleColor.Magenta     => ForegroundColors.Magenta,
                    ConsoleColor.Yellow      => ForegroundColors.Yellow,
                    ConsoleColor.White       => ForegroundColors.White,
                    _ => throw new NotImplementedException()
                }

            + color.ToString() + AnsiSequences.Reset;

            painter.Paint(colorName, position);
            position = (colorNumber % 8 == 7)
                ? (initialPosition.x, position.y + 1)
                : (position.x + columnWidth, position.y);

            colorNumber++;
        }
    }
}

public class AnsiExhibition : IExhibition
{
    private const char EscapeChr = AnsiSequences.EscapeCharacter;

    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position)
    {
        var esc = (string text) => new AnsiControlSequence(EscapeChr + text);

        foreach (var text in new AnsiString[]
        {
            "No ANSI",
            esc("[m"),
            esc("[41;3m"),
            esc("[31m") + "Red Text" + esc("[m"),
            esc("[31m") + "Red" + esc("[33m") + "Green" + esc("[m"),
            "Three " + esc("[38;2;0;0;0m") + esc("[48;2;140;150;0m") + "Separate" + esc("[m") + " " + esc("[100m") + "Words",
            esc("[38;2;255;255;255m") + "[" + esc("[38;2;0;200;0m") + "S" + esc("[38;2;100;100;100m") + "tart" + esc("[38;2;255;255;255m") + "]",
        })
        {
            Paint(text, position.x, position.y, painter);
            position.y += 3;
        }
    }

    private void Paint(AnsiString text, int left, int top, Painter painter)
    {
        var ansiExposedTextBuilder = new StringBuilder();

        foreach (var part in text.Parts)
        {
            var textToAppend = part switch
            {
                var x when x is AnsiStringTextPart => part.ToString(),
                var x when x is AnsiStringControlSequencePart seqPart => FormatForDisplay(seqPart),
                _ => throw new InvalidOperationException()
            };

            ansiExposedTextBuilder.Append(textToAppend);
        }

        painter.Paint(ansiExposedTextBuilder.ToString() + " " + ForegroundColors.Cyan + $"[{text.Length} printable characters]", (left, top));
        painter.Paint(text + AnsiSequences.Reset + " ", (left, top + 1));

        var charactersCount =
            ForegroundColors.Cyan +
            $"[{text.Length} printable characters]" +
            AnsiSequences.Reset;
    }

    private string FormatForDisplay(AnsiStringControlSequencePart part)
    {
        var escapeStr =
            ForegroundColors.Gray +
            BackgroundColors.DarkGray +
            "\\u001b" +
            ForegroundColors.Black +
            BackgroundColors.Gray;

        var partStr = part.ToString() ?? string.Empty;
        return partStr.Replace(EscapeChr.ToString(), escapeStr.ToString()) + AnsiSequences.Reset.ToString();
    }
}

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
        new (60, PaneHeights[0]),
        new (10, PaneHeights[1]),
        new (15, PaneHeights[2]),
        new (20, PaneHeights[3]),
    });

    public void Paint(Painter painter, (int x, int y) position)
    {
        var rows = new List<AnsiString>();

        var paneIdx = 0;
        foreach (var pane in Layout.Panes)
        {
            rows.Clear();
            for (int y = 0; y < pane.Size.y; y++)
            {
                var data = new AnsiString(string.Empty.PadRight(pane.Size.x));
                var dataString = data.ToString();

                if (y == 0)
                {
                    data =
                        AnsiSequences.ForegroundColors.Gray + "*  #" +
                        AnsiSequences.ForegroundColors.Blue +
                        paneIdx.ToString("00") +
                        AnsiSequences.ForegroundColors.Gray +
                        (dataString.Length > 6 ? data.ToString()[6..^1] + "*" : string.Empty);
                }
                else if (y == pane.Size.y - 1)
                {
                    data =
                        AnsiSequences.ForegroundColors.Gray + "*" +
                        AnsiSequences.ForegroundColors.Blue +
                        AnsiSequences.ForegroundColors.Gray +
                        (dataString.Length > 1 ? data.ToString()[1..^1] + "*" : string.Empty);
                }

                rows.Add(AnsiSequences.BackgroundColor(0x22, 0x33, 0x44) + data);
                painter.PaintRows(rows, pane.Position);
            }

            paneIdx++;
        }

        rows.Clear();

        paneIdx = 0;
        foreach (var pane in Layout.Panes)
        {
            var rowBuilder = new AnsiStringBuilder();
            rowBuilder
                .Append("pane ")
                .Append(AnsiSequences.BackgroundColors.Gray)
                .Append(AnsiSequences.ForegroundColors.Black)
                .Append(paneIdx.ToString("00:"))
                .Append(AnsiSequences.Reset)
                .Append($" ({pane.Position.x.ToString().PadLeft(3)}, {pane.Position.y.ToString().PadLeft(3)})   ")
                .Append($" ({pane.Size.x.ToString().PadLeft(3)} x {pane.Size.y.ToString().PadLeft(3)})   ")
            ;

            rows.Add(rowBuilder.Build());
            paneIdx++;
        }

        painter.PaintRows(rows, (2, 6));
    }
}

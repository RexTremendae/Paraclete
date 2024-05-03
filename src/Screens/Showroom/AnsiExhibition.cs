namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

public class AnsiExhibition : IExhibition
{
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int X, int Y) position, int paneIndex)
    {
        var esc = (string text) => new AnsiControlSequence($"\e{text}");

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
            Paint(text, position.X, position.Y, painter, Layout.Panes[paneIndex]);
            position.Y += 3;
        }
    }

    private static void Paint(AnsiString text, int left, int top, Painter painter, Pane pane)
    {
        var ansiExposedTextBuilder = new AnsiStringBuilder();

        foreach (var part in text.Pieces)
        {
            var textToAppend = part switch
            {
                var x when x is AnsiStringTextPiece => new AnsiString(part.ToString() ?? string.Empty),
                var x when x is AnsiStringControlSequencePart seqPart => FormatForDisplay(seqPart),
                _ => throw new InvalidOperationException(),
            };

            ansiExposedTextBuilder.Append(textToAppend);
        }

        ansiExposedTextBuilder.Append(" ").Append(AnsiSequences.ForegroundColors.Cyan).Append($"[{text.Length} printable characters]");

        painter.Paint(ansiExposedTextBuilder.Build(), pane, (left, top));
        painter.Paint(text + AnsiSequences.Reset + " ", pane, (left, top + 1));
    }

    private static AnsiString FormatForDisplay(AnsiStringControlSequencePart part)
    {
        return
            AnsiSequences.ForegroundColors.Gray +
            AnsiSequences.BackgroundColors.DarkGray +
            "\\u001b" +
            AnsiSequences.ForegroundColors.Black +
            AnsiSequences.BackgroundColors.Gray +
            (part.ToString() ?? string.Empty).Replace("\e", string.Empty) +
            AnsiSequences.Reset;
    }
}

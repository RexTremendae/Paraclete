namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

using static Paraclete.Ansi.AnsiSequences;

public class AnsiExhibition : IExhibition
{
    private const char EscapeChr = AnsiSequences.EscapeCharacter;

    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position, int paneIndex)
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
        var ansiExposedTextBuilder = new AnsiStringBuilder();

        foreach (var part in text.Pieces)
        {
            var textToAppend = part switch
            {
                var x when x is AnsiStringTextPiece => new AnsiString(part.ToString() ?? string.Empty),
                var x when x is AnsiStringControlSequencePart seqPart => FormatForDisplay(seqPart),
                _ => throw new InvalidOperationException()
            };

            ansiExposedTextBuilder.Append(textToAppend);
        }

        ansiExposedTextBuilder.Append(" ").Append(ForegroundColors.Cyan).Append($"[{text.Length} printable characters]");

        painter.Paint(ansiExposedTextBuilder.Build(), (left, top));
        painter.Paint(text + AnsiSequences.Reset + " ", (left, top + 1));

        var charactersCount =
            ForegroundColors.Cyan +
            $"[{text.Length} printable characters]" +
            AnsiSequences.Reset;
    }

    private AnsiString FormatForDisplay(AnsiStringControlSequencePart part)
    {
        return
            ForegroundColors.Gray +
            BackgroundColors.DarkGray +
            "\\u001b" +
            ForegroundColors.Black +
            BackgroundColors.Gray +
            (part.ToString() ?? string.Empty).Replace(EscapeChr.ToString(), string.Empty) +
            AnsiSequences.Reset;
    }
}

namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

using static Paraclete.Ansi.AnsiSequences;

public class ColorExhibition : IExhibition
{
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position, int paneIndex)
    {
        var columnWidth = 16;
        var colorIdx = 0;

        var rows = new List<AnsiStringBuilder>(Enumerable.Range(0, 5).Select(_ => new AnsiStringBuilder()));

        foreach (var color in Enum.GetValues<ConsoleColor>())
        {
            var fgColor = GetForegroundColorString(color);
            var bgColor = ForegroundColors.Black + GetBackgroundColorString(color);

            var rowIdx = colorIdx < 8 ? 1 : 0;
            rows[rowIdx].Append(fgColor.PadRight(columnWidth));

            rowIdx = colorIdx < 8 ? 4 : 3;
            rows[rowIdx].Append(bgColor.PadRight(columnWidth));

            colorIdx++;
        }

        painter.PaintRows(rows.Select(_ => _.Build()), Layout.Panes[0], position);
    }

    private static AnsiString GetForegroundColorString(ConsoleColor color)
    {
        return color switch {
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
            _ => throw new NotSupportedException($"Color {color} is not defined."),
        }

        + color.ToString() + AnsiSequences.Reset;
    }

    private static AnsiString GetBackgroundColorString(ConsoleColor color)
    {
        return color switch {
            ConsoleColor.Black       => BackgroundColors.Black + ForegroundColors.Gray,
            ConsoleColor.DarkBlue    => BackgroundColors.DarkBlue,
            ConsoleColor.DarkGreen   => BackgroundColors.DarkGreen,
            ConsoleColor.DarkCyan    => BackgroundColors.DarkCyan,
            ConsoleColor.DarkRed     => BackgroundColors.DarkRed,
            ConsoleColor.DarkMagenta => BackgroundColors.DarkMagenta,
            ConsoleColor.DarkYellow  => BackgroundColors.DarkYellow,
            ConsoleColor.Gray        => BackgroundColors.Gray,
            ConsoleColor.DarkGray    => BackgroundColors.DarkGray,
            ConsoleColor.Blue        => BackgroundColors.Blue,
            ConsoleColor.Green       => BackgroundColors.Green,
            ConsoleColor.Cyan        => BackgroundColors.Cyan,
            ConsoleColor.Red         => BackgroundColors.Red,
            ConsoleColor.Magenta     => BackgroundColors.Magenta,
            ConsoleColor.Yellow      => BackgroundColors.Yellow,
            ConsoleColor.White       => BackgroundColors.White,
            _ => throw new NotSupportedException($"Color {color} is not defined."),
        }

        + color.ToString() + AnsiSequences.Reset;
    }
}

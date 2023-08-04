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
        var (initialX, _) = position;

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
                ? (initialX, position.y + 1)
                : (position.x + columnWidth, position.y);

            colorNumber++;
        }
    }
}

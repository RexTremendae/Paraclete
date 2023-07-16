namespace Paraclete.Screens;

using Paraclete.Painting;

public interface IExhibition
{
    void Paint(Painter painter, (int x, int y) position);
}

public class FontExhibition : IExhibition
{
    public void Paint(Painter painter, (int x, int y) position)
    {
        var text = "0123456789:.";

        var colors = new Dictionary<Font.Size, ConsoleColor>
        {
            { Font.Size.XS, ConsoleColor.DarkBlue },
            { Font.Size.S,  ConsoleColor.Blue },
            { Font.Size.M,  ConsoleColor.DarkCyan },
            { Font.Size.L,  ConsoleColor.Cyan },
        };

        foreach (var size in Enum.GetValues<Font.Size>())
        {
            if (size == Font.Size.Undefined)
            {
                continue;
            }

            var fontWriter = FontWriter.Create(size);
            painter.Paint($"{AnsiSequences.ForegroundColors.White}{size.ToString()}:".PadRight(4), position);
            fontWriter.Write(text, colors[size], (position.x + 4, position.y), painter);
            position = (position.x, position.y + fontWriter.Font.CharacterHeight + 1);
        }
    }
}

public class ColorExhibition : IExhibition
{
    public void Paint(Painter painter, (int x, int y) position)
    {
        var initialPosition = position;
        var columnWidth = 16;

        var colorNumber = 0;

        foreach (var color in Enum.GetValues<ConsoleColor>())
        {
            var colorName = AnsiString.Create(color.ToString());
            if (color == ConsoleColor.Black)
            {
                colorName = AnsiSequences.BackgroundColors.Gray + colorName + AnsiSequences.Reset;
            }

            painter.Paint(colorName, position, color);
            position = (colorNumber % 8 == 7)
                ? (initialPosition.x, position.y + 1)
                : (position.x + columnWidth, position.y);

            colorNumber++;
        }
    }
}

public class AnsiExhibition : IExhibition
{
    public void Paint(Painter painter, (int x, int y) position)
    {
        foreach (var text in new[]
        {
            "No ANSI",
            "\\u001b[m",
            "\\u001b[41;3m",
            "\\u001b[31mRed Text\\u001b[m",
            "\\u001b[31mRed\\u001b[33mGreen\\u001b[m",
            "Three \\u001b[38;2;0;0;0m\\u001b[48;2;140;150;0mSeparate\\u001b[m \\u001b[100mWords",
            "\\u001b[38;2;255;255;255m[\\u001b[38;2;0;200;0mS\\u001b[38;2;100;100;100mtart\\u001b[38;2;255;255;255m]",
        })
        {
            Paint(text, position.x, position.y, painter);
            position.y += 3;
        }
    }

    private void Paint(string text, int left, int top, Painter painter)
    {
        var ansiExposedText = text.Replace("\\u001b", "\u001b[107m\u001b[30m\\u001b\u001b[m");
        var ansiFormattedText = text.Replace("\\u001b", "\u001b");

        painter.Paint(ansiExposedText + " ", (left, top));

        var dataLength = AnsiString.Create(ansiFormattedText).Length;
        var charactersCount =
            AnsiSequences.ForegroundColors.Cyan +
            $"[{dataLength} printable characters]" +
            AnsiSequences.Reset;

        var countTextLeft = left + text.Replace("\\", "\"").Length + 2;
        painter.Paint(charactersCount, (countTextLeft, top));
        painter.Paint(ansiFormattedText + AnsiSequences.Reset, (left, top + 1));
    }
}

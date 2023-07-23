namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Painting;

using static Paraclete.Ansi.AnsiSequences;

public class FontExhibition : IExhibition
{
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void Paint(Painter painter, (int x, int y) position, int paneIndex)
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

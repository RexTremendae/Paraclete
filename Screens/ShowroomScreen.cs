using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Showroom;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ShowroomScreen : IScreen
{
    public string Name => "Showroom";
    public ConsoleKey Shortcut => ConsoleKey.F12;

    public MenuBase Menu { get; private set; }
    public ILayout Layout { get; private set; }

    private readonly TimeWriter _currentTimeWriter;
    private readonly ExhibitionSelector _exhibitionSelector;

    public ShowroomScreen(_ShowroomMenu showroomMenu, ExhibitionSelector exhibitionSelector, OneFrameLayout layout)
    {
        Menu = showroomMenu;
        Layout = layout;
        _exhibitionSelector = exhibitionSelector;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public void PaintContent(Painter painter)
    {
        var exhibition = _exhibitionSelector.SelectedExhibition;
        exhibition.Paint(painter, (2, 5));
        var exhibitionName =
            string.Concat(exhibition.GetType().Name.RemoveEnding("Exhibition"), " exhibition") +
            $" ({_exhibitionSelector.SelectedExhibitionIndex + 1}/{_exhibitionSelector.ExhibitionCount})";

        painter.Paint(exhibitionName, (2, 2), ConsoleColor.Blue);
        painter.Paint("".PadLeft(exhibitionName.Length, '-'), (2, 3), ConsoleColor.Blue);

        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), painter);
    }
}

using Time.Menu;
using Time.Menu.Showroom;
using Time.Painting;

namespace Time.Screens;

public class ShowroomScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;
    public string Name => "Showroom";

    private readonly TimeWriter _currentTimeWriter;
    private readonly ExhibitionSelector _exhibitionSelector;

    public ShowroomScreen(_ShowroomMenu showroomMenu, ExhibitionSelector exhibitionSelector)
    {
        _exhibitionSelector = exhibitionSelector;
        _menu = showroomMenu;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public void PaintFrame(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(windowWidth-2, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == windowHeight-4)
            {
                frameRows[y] = $"╟{"".PadLeft(windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        painter.PaintRows(frameRows);
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

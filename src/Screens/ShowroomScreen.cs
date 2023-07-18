namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Showroom;
using Paraclete.Painting;

public class ShowroomScreen : IScreen
{
    private readonly TimeWriter _currentTimeWriter;
    private readonly ExhibitionSelector _exhibitionSelector;

    public ShowroomScreen(ShowroomMenu showroomMenu, ExhibitionSelector exhibitionSelector)
    {
        Menu = showroomMenu;
        _exhibitionSelector = exhibitionSelector;
        _currentTimeWriter = new TimeWriter(new ()
        {
            FontSize = Font.Size.XS,
            Color = AnsiSequences.ForegroundColors.White,
            ShowSeconds = false,
            ShowMilliseconds = false,
        });
    }

    public string Name => "Showroom";
    public ConsoleKey Shortcut => ConsoleKey.F12;

    public MenuBase Menu { get; }
    public ILayout Layout => new SinglePanelLayout();

    public void PaintContent(Painter painter, int windowWidth, int windowHeight)
    {
        var exhibition = _exhibitionSelector.SelectedExhibition;
        exhibition.Paint(painter, (2, 5));
        var exhibitionName =
            string.Concat(exhibition.GetType().Name.RemoveEnding("Exhibition"), " exhibition") +
            $" ({_exhibitionSelector.SelectedExhibitionIndex + 1}/{_exhibitionSelector.ExhibitionCount})";

        painter.PaintRows(
            new[]
            {
                AnsiSequences.ForegroundColors.Blue + exhibitionName,
                string.Empty.PadLeft(exhibitionName.Length, '-'),
            },
            (2, 2));

        _currentTimeWriter.Write(DateTime.Now, (-7, 1), painter);
    }
}

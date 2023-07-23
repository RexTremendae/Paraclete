namespace Paraclete.Screens.Showroom;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Showroom;
using Paraclete.Painting;

public class ShowroomScreen : IScreen
{
    private readonly ExhibitionSelector _exhibitionSelector;

    public ShowroomScreen(ShowroomMenu showroomMenu, ExhibitionSelector exhibitionSelector)
    {
        Menu = showroomMenu;
        _exhibitionSelector = exhibitionSelector;
    }

    public string Name => "Showroom";
    public ConsoleKey Shortcut => ConsoleKey.F12;

    public MenuBase Menu { get; }
    public ILayout Layout => _exhibitionSelector.SelectedExhibition.Layout;

    public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
    () =>
    {
        var exhibition = _exhibitionSelector.SelectedExhibition;
        exhibition.Paint(painter, (2, 5), paneIndex);
        var exhibitionName = GetExhibitionNameString(exhibition);

        if (paneIndex == 0)
        {
            painter.PaintRows(
                new[]
                {
                    AnsiSequences.ForegroundColors.Blue + exhibitionName,
                    string.Empty.PadLeft(exhibitionName.Length, '-'),
                },
                (2, 2));
        }
    };

    private string GetExhibitionNameString(IExhibition exhibition)
    {
        var camelCaseName = exhibition.GetType().Name;
        var words = new List<string>();

        var start = 0;

        for (int idx = 0; idx < camelCaseName.Length; idx++)
        {
            var chr = camelCaseName[idx];
            if (!char.IsUpper(chr) || start == idx)
            {
                continue;
            }

            words.Add(camelCaseName[start..idx]);
            start = idx;
        }

        words.Add(camelCaseName[start..].ToLower());

        var pageIdx = $" ({_exhibitionSelector.SelectedExhibitionIndex + 1}/{_exhibitionSelector.ExhibitionCount})";
        var title = string.Join(" ", words) + pageIdx;

        return char.ToUpper(title[0]) + title[1..];
    }
}

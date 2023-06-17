using Paraclete.Screens;
using Paraclete.Menu;

namespace Paraclete.Painting;

public class MenuPainter
{
    private readonly ScreenSelector _screenSelector;
    private readonly IServiceProvider _services;

    public MenuPainter(ScreenSelector screenSelector, IServiceProvider services)
    {
        _screenSelector = screenSelector;
        _services = services;
    }

    public void PaintMenu(Painter painter)
    {
        var selectedScreen = _screenSelector.SelectedScreen;
        var rows = new PaintRow[2];

        rows[0] = GetScreenSelectionMenuRowParts(selectedScreen);
        rows[1] = GetSelectedScreenMenuRowParts(selectedScreen.Menu.MenuItems);

        painter.PaintRows(rows, (2, Console.WindowHeight-3));
    }

    private PaintRow GetSelectedScreenMenuRowParts(IReadOnlyDictionary<ConsoleKey, ICommand> menuItems)
    {
        var row = new List<PaintSection>();

        var isFirst = true;
        foreach (var (key, description) in menuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (!isFirst)
            {
                row.Add(new ("  ", ConsoleColor.Gray));
            }

            row.AddRange(GetMenuParts(key, description));

            isFirst = false;
        }

        return new (row.ToArray());
    }

    private PaintRow GetScreenSelectionMenuRowParts(IScreen selectedScreen)
    {
        var row = new List<PaintSection>();

        var isFirst = true;

        row.Add(new ("【", ConsoleColor.White));

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services).OrderBy(_ => _.Shortcut))
        {
            var isSelected = selectedScreen.Name == screen.Name;
            var format = isSelected
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;
            var label = $"{format} {screen.Name} {AnsiSequences.Reset}";

            if (!isFirst)
            {
                row.Add(new (" · ", ConsoleColor.White));
            }

            row.Add(new ($"{screen.Shortcut.ToString()} ", ConsoleColor.Green));
            row.Add(new (label, ConsoleColor.Black));

            isFirst = false;
        }

        row.Add(new ("】", ConsoleColor.White));

        return new (row.ToArray());
    }

    private IEnumerable<PaintSection> GetMenuParts(ConsoleKey key, string description)
    {
        var bracketColor = ConsoleColor.White;
        var shortcutColor = ConsoleColor.Green;
        var descriptionColor = ConsoleColor.Gray;

        var sections = new List<PaintSection>();

        var startBracketIndex = description.IndexOf('[');
        var endBracketIndex = -1;
        if (startBracketIndex >= 0)
        {
            endBracketIndex = description.IndexOf(']', startBracketIndex);
        }

        var explicitBrackets = startBracketIndex >= 0 && endBracketIndex >= 0;

        sections.Add(new ("【", bracketColor));

        if (explicitBrackets)
        {
            sections.Add(new (description[..startBracketIndex], descriptionColor));
        }

        sections.Add(new (key.ToDisplayString() + (explicitBrackets ? "" : " "), shortcutColor));
        sections.Add(new (description[(endBracketIndex+1)..], descriptionColor));
        sections.Add(new ("】", bracketColor));

        return sections;
    }
}

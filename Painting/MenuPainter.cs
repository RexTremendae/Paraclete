using Paraclete.Screens;

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
        var menuItems = selectedScreen.Menu.MenuItems;
        var rows = Enumerable.Range(0, 2)
            .Select<int, (List<string> parts, List<ConsoleColor> colors)>(_ => new (new List<string>(), new List<ConsoleColor>()))
            .ToArray();

        var first = true;
        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            var isSelected = selectedScreen.Name == screen.Name;
            var format = isSelected
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue;
            var startingBracket = isSelected ? "[" : " ";
            var endingBracket = isSelected ? "]" : " ";
            var label = $"{format}{startingBracket} {screen.Name} {endingBracket}{AnsiSequences.Reset}  ";
            if (!first) { label = "  " + label; }
            rows[0].parts.Add(label);
            rows[0].colors.Add(ConsoleColor.Black);
            first = false;
        }

        var isFirst = true;
        foreach (var (key, description) in menuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                rows[1].parts.Add("    ");
                rows[1].colors.Add(ConsoleColor.Gray);
            }

            var (parts, colors) = GetMenuParts(key, description);
            rows[1].parts.AddRange(parts);
            rows[1].colors.AddRange(colors);
        }

        var paintableRows = rows.Select(_ => ((IEnumerable<string>)_.parts, (IEnumerable<ConsoleColor>)_.colors)).ToArray();
        painter.PaintRows(paintableRows, (2, Console.WindowHeight-3));
    }

    private (IEnumerable<string> parts, IEnumerable<ConsoleColor> colors) GetMenuParts(ConsoleKey key, string description)
    {
        var bracketColor = ConsoleColor.White;
        var shortcutColor = ConsoleColor.Green;
        var descriptionColor = ConsoleColor.Gray;

        var parts = new List<string>();
        var colors = new List<ConsoleColor>();

        var startBracketIndex = description.IndexOf('[');
        var endBracketIndex = -1;
        if (startBracketIndex >= 0)
        {
            endBracketIndex = description.IndexOf(']', startBracketIndex);
        }

        var explicitBrackets = startBracketIndex >= 0 && endBracketIndex >= 0;

        parts.Add("[");
        colors.Add(bracketColor);

        if (explicitBrackets)
        {
            parts.Add(description[..startBracketIndex]);
            colors.Add(descriptionColor);
        }

        parts.Add(key.ToDisplayString() + (explicitBrackets ? "" : " "));
        colors.Add(shortcutColor);

        parts.Add(description[(endBracketIndex+1)..]);
        colors.Add(descriptionColor);

        parts.Add("]");
        colors.Add(bracketColor);

        return (parts, colors);
    }
}

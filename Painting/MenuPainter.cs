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
        var rows = Enumerable.Range(0, 2)
            .Select<int, (List<string> parts, List<ConsoleColor> colors)>(_ => new (new List<string>(), new List<ConsoleColor>()))
            .ToArray();

        rows[0] = GetScreenSelectionMenuRowParts(selectedScreen);
        rows[1] = GetSelectedScreenMenuRowParts(selectedScreen.Menu.MenuItems);

        var paintableRows = rows.Select(_ => ((IEnumerable<string>)_.parts, (IEnumerable<ConsoleColor>)_.colors)).ToArray();
        painter.PaintRows(paintableRows, (2, Console.WindowHeight-3));
    }

    private (List<string> parts, List<ConsoleColor> colors) GetSelectedScreenMenuRowParts(IReadOnlyDictionary<ConsoleKey, ICommand> menuItems)
    {
        var row = (parts: new List<string>(), colors: new List<ConsoleColor>());

        var isFirst = true;
        foreach (var (key, description) in menuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (!isFirst)
            {
                row.parts.Add("  ");
                row.colors.Add(ConsoleColor.Gray);
            }

            var (parts, colors) = GetMenuParts(key, description);
            row.parts.AddRange(parts);
            row.colors.AddRange(colors);

            isFirst = false;
        }

        return row;
    }

    private (List<string> parts, List<ConsoleColor> colors) GetScreenSelectionMenuRowParts(IScreen selectedScreen)
    {
        var row = (parts: new List<string>(), colors: new List<ConsoleColor>());

        var isFirst = true;

        row.parts.Add("【");
        row.colors.Add(ConsoleColor.White);

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services).OrderBy(_ => _.Shortcut))
        {
            var isSelected = selectedScreen.Name == screen.Name;
            var format = isSelected
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;
            var label = $"{format} {screen.Name} {AnsiSequences.Reset}";

            if (!isFirst)
            {
                row.parts.Add(" · ");
                row.colors.Add(ConsoleColor.White);
            }

            row.parts.Add($"{screen.Shortcut.ToString()} ");
            row.colors.Add(ConsoleColor.Green);

            row.parts.Add(label);
            row.colors.Add(ConsoleColor.Black);

            isFirst = false;
        }

        row.parts.Add("】");
        row.colors.Add(ConsoleColor.White);

        return row;
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

        parts.Add("【");
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

        parts.Add("】");
        colors.Add(bracketColor);

        return (parts, colors);
    }
}

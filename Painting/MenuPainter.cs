using Paraclete.Screens;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;

namespace Paraclete.Painting;

public class MenuPainter
{
    private readonly ScreenSelector _screenSelector;
    private readonly IServiceProvider _services;
    private readonly _ShortcutsMenu _shortcutsMenu;

    private readonly ConsoleColor _bracketColor = ConsoleColor.White;
    private readonly ConsoleColor _shortcutColor = ConsoleColor.Green;
    private readonly ConsoleColor _textColor = ConsoleColor.Gray;

    public MenuPainter(ScreenSelector screenSelector, IServiceProvider services, _ShortcutsMenu shortcutsMenu)
    {
        _screenSelector = screenSelector;
        _services = services;
        _shortcutsMenu = shortcutsMenu;
    }

    public void PaintMenu(Painter painter, bool shortcutsMenuActive, int windowWidth)
    {
        var selectedScreen = _screenSelector.SelectedScreen;
        var rows = new AnsiString[2];

        var paddingWidth = windowWidth - 3;
        rows[0] = GetScreenSelectionMenuRowParts(selectedScreen, shortcutsMenuActive).Build().PadRight(paddingWidth);
        rows[1] = GetSelectedScreenMenuRowParts(shortcutsMenuActive ? _shortcutsMenu : selectedScreen.Menu).Build().PadRight(paddingWidth);

        painter.PaintRows(rows, (2, Console.WindowHeight-3));
    }

    private AnsiStringBuilder GetSelectedScreenMenuRowParts(MenuBase menu)
    {
        var row = new AnsiStringBuilder();

        var isFirst = true;
        foreach (var (key, description) in menu.MenuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (!isFirst)
            {
                row.Append(("  ", ConsoleColor.Gray));
            }

            row.Append(GetMenuParts(key, description));

            isFirst = false;
        }

        return row;
    }

    private AnsiStringBuilder GetScreenSelectionMenuRowParts(IScreen selectedScreen, bool shortcutsMenuActive)
    {
        var row = new AnsiStringBuilder();

        row.Append(("[", _bracketColor));
        row.Append(("TAB ", _shortcutColor));

        var format = (shortcutsMenuActive)
            ? AnsiSequences.BackgroundColors.Blue
            : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;
        row.Append(($"{format} Quick menu {AnsiSequences.Reset}", foregroundColor: ConsoleColor.Black));

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services).OrderBy(_ => _.Shortcut))
        {
            var isSelected = selectedScreen.Name == screen.Name;
            format = (isSelected && !shortcutsMenuActive)
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;
            var label = $"{format} {screen.Name} {AnsiSequences.Reset}";

            row.Append((" · ", _bracketColor));

            row.Append(($"{screen.Shortcut.ToString()} ", _shortcutColor));
            row.Append((label, ConsoleColor.Black));
        }

        row.Append(("]", _bracketColor));

        return row;
    }

    private IEnumerable<PaintSection> GetMenuParts(ConsoleKey key, string description)
    {
        var sections = new List<PaintSection>();

        var startBracketIndex = description.IndexOf('[');
        var endBracketIndex = -1;
        if (startBracketIndex >= 0)
        {
            endBracketIndex = description.IndexOf(']', startBracketIndex);
        }

        var explicitBrackets = startBracketIndex >= 0 && endBracketIndex >= 0;

        sections.Add(new ("[", ForegroundColor: _bracketColor, BackgroundColor: null));

        if (explicitBrackets)
        {
            sections.Add(new (description[..startBracketIndex], ForegroundColor: _textColor, BackgroundColor: null));
        }

        var shortcutText = key.ToDisplayString() + (explicitBrackets ? string.Empty : " ");
        sections.Add(new (shortcutText, ForegroundColor: _shortcutColor, BackgroundColor: null));
        sections.Add(new (description[(endBracketIndex+1)..], ForegroundColor: _textColor, BackgroundColor: null));
        sections.Add(new ("]", ForegroundColor: _bracketColor, BackgroundColor: null));

        return sections;
    }
}

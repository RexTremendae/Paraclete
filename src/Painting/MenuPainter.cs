namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Screens;

public class MenuPainter
{
    private readonly ScreenSelector _screenSelector;
    private readonly IServiceProvider _services;
    private readonly ShortcutsMenu _shortcutsMenu;

    private readonly AnsiControlSequence _bracketColor = AnsiSequences.ForegroundColors.White;
    private readonly AnsiControlSequence _shortcutColor = AnsiSequences.ForegroundColors.Green;
    private readonly AnsiControlSequence _textColor = AnsiSequences.ForegroundColors.Gray;

    public MenuPainter(ScreenSelector screenSelector, IServiceProvider services, ShortcutsMenu shortcutsMenu)
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
        rows[0] = GetScreenSelectionMenuRowParts(selectedScreen, shortcutsMenuActive).Build().ToString().PadRight(paddingWidth);
        rows[1] = GetSelectedScreenMenuRowParts(shortcutsMenuActive ? _shortcutsMenu : selectedScreen.Menu).Build().ToString().PadRight(paddingWidth);

        painter.PaintRows(rows, (2, Console.WindowHeight - 3));
    }

    private AnsiStringBuilder GetSelectedScreenMenuRowParts(MenuBase menu)
    {
        var row = new AnsiStringBuilder();

        var isFirst = true;
        foreach (var (key, description) in menu.MenuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (!isFirst)
            {
                row.Append("  ");
            }

            AppendMenuParts(row, key, description);

            isFirst = false;
        }

        return row;
    }

    private AnsiStringBuilder GetScreenSelectionMenuRowParts(IScreen selectedScreen, bool shortcutsMenuActive)
    {
        var row = new AnsiStringBuilder();

        row
            .Append(_bracketColor).Append("[")
            .Append(_shortcutColor).Append("TAB ");

        var format = shortcutsMenuActive
            ? AnsiSequences.BackgroundColors.Blue
            : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;

        row
            .Append(AnsiSequences.ForegroundColors.Black)
            .Append(format).Append(" Quick menu ").Append(AnsiSequences.Reset);

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services).OrderBy(_ => _.Shortcut))
        {
            var isSelected = selectedScreen.Name == screen.Name;
            format = (isSelected && !shortcutsMenuActive)
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;
            var label = $"{format} {screen.Name} {AnsiSequences.Reset}";

            row.Append(_bracketColor).Append(" · ");
            row.Append(_shortcutColor).Append($"{screen.Shortcut.ToString()} ");
            row.Append(AnsiSequences.ForegroundColors.Black).Append(label);
        }

        row.Append(_bracketColor).Append("]");

        return row;
    }

    private void AppendMenuParts(AnsiStringBuilder builder, ConsoleKey key, string description)
    {
        var startBracketIndex = description.IndexOf('[');
        var endBracketIndex = -1;
        if (startBracketIndex >= 0)
        {
            endBracketIndex = description.IndexOf(']', startBracketIndex);
        }

        var explicitBrackets = startBracketIndex >= 0 && endBracketIndex >= 0;

        builder.Append(_bracketColor).Append("[");

        if (explicitBrackets)
        {
            builder.Append(_textColor).Append(description[..startBracketIndex]);
        }

        var shortcutText = key.ToDisplayString() + (explicitBrackets ? string.Empty : " ");
        builder.Append(_shortcutColor).Append(shortcutText);
        builder.Append(_textColor).Append(description[(endBracketIndex + 1)..]);
        builder.Append(_bracketColor).Append("]");
    }
}

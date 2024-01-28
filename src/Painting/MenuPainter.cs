namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Screens;

public class MenuPainter(ScreenSelector screenSelector, IServiceProvider services, ShortcutsMenu shortcutsMenu)
{
    private readonly ScreenSelector _screenSelector = screenSelector;
    private readonly IServiceProvider _services = services;
    private readonly ShortcutsMenu _shortcutsMenu = shortcutsMenu;

    private readonly AnsiControlSequence _bracketColor = AnsiSequences.ForegroundColors.White;
    private readonly AnsiControlSequence _shortcutColor = AnsiSequences.ForegroundColors.Green;
    private readonly AnsiControlSequence _textColor = AnsiSequences.ForegroundColors.Gray;

    public void PaintMenu(Painter painter, bool shortcutsMenuActive, int windowWidth)
    {
        var selectedScreen = _screenSelector.SelectedScreen;
        var rows = new AnsiString[2];

        var paddingWidth = windowWidth - 3;

        rows[0] = GetScreenSelectionMenuRowParts(selectedScreen, shortcutsMenuActive)
            .Build()
            .PadRight(paddingWidth);

        rows[1] = GetSelectedScreenMenuRowParts(shortcutsMenuActive ? _shortcutsMenu : selectedScreen.Menu)
            .Build()
            .PadRight(paddingWidth);

        painter.PaintRows(rows, (2, -3), (-1, -1));
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

        var appendFormat = (string text, bool isSelected) =>
        {
            var fgColor = isSelected
                ? AnsiSequences.ForegroundColors.Black
                : AnsiSequences.ForegroundColors.Blue;

            var bgColor = isSelected
                ? AnsiSequences.BackgroundColors.Blue
                : AnsiSequences.BackgroundColors.DarkBlue;

            row
                .Append(fgColor)
                .Append(bgColor)
                .Append($" {text} ")
                .Append(AnsiSequences.Reset);
        };

        appendFormat("Quick menu", shortcutsMenuActive);

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services).OrderBy(_ => _.Shortcut))
        {
            row
                .Append(_bracketColor).Append(" · ")
                .Append(_shortcutColor).Append($"{screen.Shortcut} ");

            var isSelected = selectedScreen.Name == screen.Name;
            appendFormat(screen.Name, isSelected && !shortcutsMenuActive);
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

namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;

public class ShortcutsScreen(ShortcutsMenu shortcutsMenu) : IScreen
{
    public string Name => "Shortcuts";
    public ConsoleKey Shortcut => ConsoleKey.F7;

    public MenuBase Menu { get; } = shortcutsMenu;
    public ILayout Layout { get; } = new SinglePaneLayout();

    public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
    () =>
    {
        var rows = new List<AnsiString>();
        foreach (var (key, command) in Menu.MenuItems)
        {
            var shortcut = (IShortcut)command;
            var description = AnsiSequences.ForegroundColors.White + (command is CustomShortcutCommand
                ? "Custom command: " + AnsiSequences.ForegroundColors.Gray + shortcut.LongDescription
                : shortcut.LongDescription);

            var row =
                AnsiSequences.ForegroundColors.Gray + "[" +
                AnsiSequences.ForegroundColors.Green + key.ToString() +
                AnsiSequences.ForegroundColors.Gray + "] " +
                description;

            rows.Add(row);
        }

        painter.PaintRows(rows, (2, 2));
    };
}

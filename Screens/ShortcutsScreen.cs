using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ShortcutsScreen : IScreen
{
    public string Name => "Shortcuts";
    public ConsoleKey Shortcut => ConsoleKey.F3;

    public MenuBase Menu { get; }
    public ILayout Layout => new SinglePanelLayout();

    private ScreenInvalidator _screenInvalidator;
    private readonly TimeWriter _currentTimeWriter;

    public ShortcutsScreen(ScreenInvalidator screenInvalidator, _ShortcutsMenu shortcutsMenu)
    {
        _screenInvalidator = screenInvalidator;

        _currentTimeWriter = new TimeWriter(new() {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });

        Menu = shortcutsMenu;
    }

    public void PaintContent(Painter painter)
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

        painter.PaintRows(rows.ToArray(), (2, 2));

        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), painter);
    }
}

namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;

public class ShortcutsScreen : IScreen
{
    private readonly TimeWriter _currentTimeWriter;
    private ScreenInvalidator _screenInvalidator;

    public ShortcutsScreen(ScreenInvalidator screenInvalidator, ShortcutsMenu shortcutsMenu)
    {
        _screenInvalidator = screenInvalidator;

        _currentTimeWriter = new TimeWriter(new ()
        {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false,
        });

        Menu = shortcutsMenu;
    }

    public string Name => "Shortcuts";
    public ConsoleKey Shortcut => ConsoleKey.F3;

    public MenuBase Menu { get; }
    public ILayout Layout => new SinglePanelLayout();

    public void PaintContent(Painter painter, int windowWidth, int windowHeight)
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

        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth - 7, 1), painter);
    }
}

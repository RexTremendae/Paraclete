using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ShortcutsScreen : IScreen
{
    public string Name => "Shortcuts";
    public int Ordinal => 20;
    public ConsoleKey Shortcut => ConsoleKey.F3;

    public MenuBase Menu { get; private set; }

    private OneFrameLayout _layout = new();
    public ILayout Layout => _layout;

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
            var row =
                AnsiSequences.ForegroundColors.Gray + "[" +
                AnsiSequences.ForegroundColors.Green + key.ToString() +
                AnsiSequences.ForegroundColors.Gray + "] " +
                AnsiSequences.ForegroundColors.White + shortcut.LongDescription;
            rows.Add(row);
        }

        painter.PaintRows(rows.ToArray(), (2, 2));

        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), painter);
    }
}

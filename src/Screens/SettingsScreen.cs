namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Painting;

public class SettingsScreen : IScreen
{
    public SettingsScreen(SettingsMenu menu)
    {
        Menu = menu;
        Layout = new SinglePaneLayout();
    }

    public MenuBase Menu { get; }

    public ILayout Layout { get; }

    public string Name => "Settings";

    public ConsoleKey Shortcut => ConsoleKey.F8;

    public Action GetPaintPaneAction(Painter painter, int paneIndex)
    {
        return () => { };
    }
}

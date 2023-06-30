namespace Paraclete.Menu.Shortcuts;

public class OutlookCommand : StartProcessCommandBase, IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.O;
    public string Description => "[O]utlook";
    public string LongDescription => "Start Outlook with recycle switch";
    public async Task Execute() => await Execute("outlook", "/recycle");
}

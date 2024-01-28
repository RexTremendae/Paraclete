namespace Paraclete.Menu.Shortcuts;

public class OutlookCommand
    : IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.O;
    public string Description => "[O]utlook";
    public string LongDescription => "Start Outlook with recycle switch";

    public Task Execute() => ProcessRunner.ExecuteAsync(
        "outlook",
        args: ["/recycle"],
        launchExternal: true);
}

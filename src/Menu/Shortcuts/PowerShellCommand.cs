namespace Paraclete.Menu.Shortcuts;

public class PowerShellCommand
    : IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.P;
    public string Description => "[P]owerShell";
    public string LongDescription => "Start new PowerShell instance";

    public Task Execute() => ProcessRunner.ExecuteAsync(
        "powershell",
        launchExternal: true);
}

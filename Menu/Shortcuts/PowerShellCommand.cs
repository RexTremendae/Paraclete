namespace Paraclete.Menu.Shortcuts;

public class PowerShellCommand : StartProcessCommandBase, IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.P;
    public string Description => "[P]owerShell";
    public string LongDescription => "Start new PowerShell instance";
    public async Task Execute() => await base.Execute("powershell");
}

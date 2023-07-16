namespace Paraclete.Menu.Shortcuts;

public class StartTaskManagerCommand : StartProcessCommandBase, IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.T;
    public string Description => "[T]askmgr";
    public string LongDescription => "Start task manager";
    public async Task Execute() => await Execute("taskmgr");
}

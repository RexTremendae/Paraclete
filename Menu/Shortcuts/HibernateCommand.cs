namespace Paraclete.Menu.Shortcuts;

public class HibernateCommand : StartProcessCommandBase, IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.H;
    public string Description => "[H]ibernate";
    public string LongDescription => "Hibernate";
    public async Task Execute() => await base.Execute("shutdown", new[] { "/h" });
}

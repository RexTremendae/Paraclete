namespace Paraclete.Menu.Shortcuts;

public class HibernateCommand
    : IShortcut
{
    public ConsoleKey Shortcut => ConsoleKey.H;
    public string Description => "[H]ibernate";
    public string LongDescription => "Hibernate";

    public Task Execute() => ProcessRunner.ExecuteAsync(
        "shutdown",
        args: ["/h"],
        launchExternal: true);
}

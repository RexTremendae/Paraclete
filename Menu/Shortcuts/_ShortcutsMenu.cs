namespace Paraclete.Menu.Shortcuts;

public class _ShortcutsMenu : MenuBase
{
    public _ShortcutsMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartTaskManagerCommand),
        typeof(HibernateCommand),
        typeof(PowerShellCommand)
    })
    {}
}

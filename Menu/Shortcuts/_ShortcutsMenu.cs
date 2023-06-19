namespace Paraclete.Menu.Shortcuts;

public class _ShortcutsMenu : MenuBase
{
    public _ShortcutsMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartTaskManagerCommand),
        typeof(OutlookCommand),
        typeof(PowerShellCommand),
        typeof(HibernateCommand)
    })
    {}
}

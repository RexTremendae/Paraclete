using Paraclete.Menu.Shortcuts;

namespace Paraclete.Menu;

public class _ShortcutsMenu : MenuBase
{
    public _ShortcutsMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(StartTaskManagerCommand),
        typeof(HibernateCommand)
    })
    {}
}

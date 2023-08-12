namespace Paraclete.Menu.Unicode;

public class UnicodeMenu : MenuBase
{
    public UnicodeMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(UnicodeUpCommand),
        typeof(UnicodeDownCommand),
        typeof(SelectNextColumnCommand),
        typeof(SelectPreviousColumnCommand),
        typeof(SetStartCodepointCommand),
    })
    {
    }
}

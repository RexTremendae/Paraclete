namespace Paraclete.Menu.Unicode;

public class UnicodeMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(UnicodeUpCommand),
        typeof(UnicodeDownCommand),
        typeof(SelectNextColumnCommand),
        typeof(SelectPreviousColumnCommand),
        typeof(SetStartCodepointCommand),
    ])
{
}

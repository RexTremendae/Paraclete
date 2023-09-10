namespace Paraclete.Menu.Notes;

public class NotesMenu : MenuBase
{
    public NotesMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(NextSectionCommand),
        typeof(PreviousSectionCommand),
    })
    {
    }
}

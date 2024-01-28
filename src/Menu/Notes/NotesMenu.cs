namespace Paraclete.Menu.Notes;

public class NotesMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(NextSectionCommand),
        typeof(PreviousSectionCommand),
    })
{
}

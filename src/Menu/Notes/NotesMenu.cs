namespace Paraclete.Menu.Notes;

public class NotesMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(NextSectionCommand),
        typeof(PreviousSectionCommand),
    ])
{
}

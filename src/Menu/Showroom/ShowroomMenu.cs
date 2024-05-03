namespace Paraclete.Menu.Showroom;

public class ShowroomMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(TriggerScreenSaverCommand),
        typeof(PreviousExhibitionCommand),
        typeof(NextExhibitionCommand),
    ])
{
}

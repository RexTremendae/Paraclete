namespace Paraclete.Menu.Showroom;

public class ShowroomMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(TriggerScreenSaverCommand),
        typeof(PreviousExhibitionCommand),
        typeof(NextExhibitionCommand),
    })
{
}

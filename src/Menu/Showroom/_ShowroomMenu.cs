namespace Paraclete.Menu.Showroom;

public class ShowroomMenu : MenuBase
{
    public ShowroomMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(TriggerScreenSaverCommand),
        typeof(PreviousExhibitionCommand),
        typeof(NextExhibitionCommand),
    })
    {
    }
}

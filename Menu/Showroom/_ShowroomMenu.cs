namespace Time.Menu.Showroom;

public class _ShowroomMenu : MenuBase
{
    public _ShowroomMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(GotoMainMenuCommand),
        typeof(TriggerScreenSaverCommand),
        typeof(NextExhibitionCommand),
        typeof(PreviousExhibitionCommand)
    })
    {}
}

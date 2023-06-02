namespace Time.Menu.Todo;

public class _ShowroomMenu : MenuBase
{
    public _ShowroomMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(GotoMainMenuCommand)
    })
    {}
}

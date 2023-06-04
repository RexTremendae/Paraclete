namespace Time.Menu.Todo;

public class _TodoMenu : MenuBase
{
    public _TodoMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(AddTodoItemCommand)
    })
    {}
}

namespace Paraclete.Menu.Chess;

public class ChessMenu : MenuBase, IInitializer
{
    public ChessMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        /*
        typeof(XxxCommand),
        */
    })
    {
    }

    public Task Initialize()
    {
        return Task.CompletedTask;
    }
}

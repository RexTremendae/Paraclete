using Microsoft.Extensions.DependencyInjection;

namespace Paraclete.Menu;

public abstract class MenuBase
{
    public IReadOnlyDictionary<ConsoleKey, ICommand> MenuItems { get; }

    public MenuBase(IServiceProvider services, Type[] commands)
    {
        var commandInstances = commands.Select(_ => services.GetRequiredService(_)).OfType<ICommand>();
        MenuItems = commandInstances.ToDictionary(key => key.Shortcut, value => value);
    }
}

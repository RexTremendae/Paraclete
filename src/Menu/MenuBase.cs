namespace Paraclete.Menu;

using Microsoft.Extensions.DependencyInjection;

public abstract class MenuBase
{
    private readonly Dictionary<ConsoleKey, ICommand> _menuItems = [];

    protected MenuBase(IServiceProvider services, Type[] commands)
    {
        foreach (var command in commands.Select(services.GetRequiredService).OfType<ICommand>())
        {
            _menuItems.Add(command.Shortcut, command);
        }
    }

    public IReadOnlyDictionary<ConsoleKey, ICommand> MenuItems => _menuItems;

    protected void AddCommand(ConsoleKey key, ICommand command)
    {
        _menuItems.Add(key, command);
    }
}

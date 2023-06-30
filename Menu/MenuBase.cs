namespace Paraclete.Menu;

using Microsoft.Extensions.DependencyInjection;

public abstract class MenuBase
{
    private readonly Dictionary<ConsoleKey, ICommand> _menuItems;
    public IReadOnlyDictionary<ConsoleKey, ICommand> MenuItems => _menuItems;

    public MenuBase(IServiceProvider services, Type[] commands)
    {
        _menuItems = new ();
        foreach (var command in commands.Select(_ => services.GetRequiredService(_)).OfType<ICommand>())
        {
            _menuItems.Add(command.Shortcut, command);
        }
    }

    public void AddCommand(ConsoleKey key, ICommand command)
    {
        _menuItems.Add(key, command);
    }
}

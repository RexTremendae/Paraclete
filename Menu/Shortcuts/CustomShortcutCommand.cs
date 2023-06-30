namespace Paraclete.Menu.Shortcuts;

public class CustomShortcutCommand : StartProcessCommandBase, IShortcut
{
    private readonly string _command;
    private readonly string[] _arguments;

    public CustomShortcutCommand(ConsoleKey shortcut, string shortDescription, string longDescription, string command, params string[] arguments)
    {
        _command = command;
        _arguments = arguments;
        Shortcut = shortcut;
        Description = shortDescription;
        LongDescription = longDescription;
    }

    public ConsoleKey Shortcut { get; }
    public string Description { get; }
    public string LongDescription { get; }

    public async Task Execute() => await Execute(_command, _arguments);
}

namespace Paraclete.Menu.Shortcuts;

[ExcludeFromEnumeration]
public class CustomShortcutCommand(ConsoleKey shortcut, string shortDescription, string longDescription, string command, params string[] arguments)
    : StartProcessCommandBase, IShortcut
{
    private readonly string _command = command;
    private readonly string[] _arguments = arguments;

    public ConsoleKey Shortcut { get; } = shortcut;
    public string Description { get; } = shortDescription;
    public string LongDescription { get; } = longDescription;

    public async Task Execute() => await Execute(_command, _arguments);
}

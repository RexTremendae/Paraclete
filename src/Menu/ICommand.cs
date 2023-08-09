namespace Paraclete.Menu;

public interface ICommand
{
    public static readonly ICommand NoCommand = new NoCommandImplementation();

    ConsoleKey Shortcut { get; }
    virtual bool IsScreenSaverResistant => false;
    string Description { get; }

    Task Execute();

    [ExcludeFromEnumeration]
    private sealed class NoCommandImplementation : ICommand
    {
        public ConsoleKey Shortcut => ConsoleKey.NoName;

        public string Description => string.Empty;

        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}

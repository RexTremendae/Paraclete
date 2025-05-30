namespace Paraclete.Menu;

public interface ICommand
{
    static readonly ICommand NoCommand = new NoCommandImplementation();

    virtual bool IsScreenSaverResistant => false;

    ConsoleKey Shortcut { get; }
    string Description { get; }

    abstract Task Execute();

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

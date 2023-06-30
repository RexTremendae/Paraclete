namespace Paraclete.Menu;

public interface ICommand
{
    public static ICommand NoCommand = new NoCommandImplementation();

    ConsoleKey Shortcut { get; }
    virtual bool IsScreenSaverResistant => false;
    string Description { get; }

    Task Execute();

    [ExcludeFromEnumeration]
    private class NoCommandImplementation : ICommand
    {
        public ConsoleKey Shortcut => ConsoleKey.NoName;

        public string Description => string.Empty;

        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}

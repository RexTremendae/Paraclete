namespace Paraclete.Menu;

public interface ICommand
{
    ConsoleKey Shortcut { get; }
    virtual bool IsScreenSaverResistant => false;
    string Description { get; }
    Task Execute();

    public static ICommand NoCommand = new NoCommandImplementation();

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

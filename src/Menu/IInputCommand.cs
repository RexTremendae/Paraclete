namespace Paraclete.Menu;

public interface IInputCommand<in T> : IInputCommand
{
    Task CompleteInput(T data);
}

public interface IInputCommand : ICommand
{
    static readonly IInputCommand NoInputCommand = new NoInputCommandImplementation();

    [ExcludeFromEnumeration]
    private sealed class NoInputCommandImplementation : IInputCommand
    {
        public ConsoleKey Shortcut => throw new NotSupportedException();

        public string Description => throw new NotSupportedException();

        public Task Execute()
        {
            throw new NotSupportedException();
        }
    }
}

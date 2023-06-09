namespace Paraclete.Menu;

public interface IInputCommand<T> : IInputCommand
{
    Task CompleteInput(T data);
}

public interface IInputCommand : ICommand
{
    public static readonly IInputCommand NoInputCommand = new NoInputCommandImplementation();

    [ExcludeFromEnumeration]
    private class NoInputCommandImplementation : IInputCommand
    {
        public ConsoleKey Shortcut => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}

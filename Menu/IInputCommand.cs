namespace Paraclete.Menu;

public interface IInputCommand<T> : IInputCommand
{
    Task CompleteInput(T data);
}

public interface IInputCommand : ICommand
{
}

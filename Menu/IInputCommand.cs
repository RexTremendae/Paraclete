namespace Paraclete.Menu;

public interface IInputCommand<T> : IInputCommand
{
    void CompleteInput(T data);
}

public interface IInputCommand : ICommand
{
}

namespace Time.Menu;

public interface ICommand
{
    ConsoleKey Shortcut { get; }
    string Description { get; }
    Task Execute();
}

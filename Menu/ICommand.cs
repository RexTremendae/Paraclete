namespace Time.Menu;

public interface ICommand
{
    MenuCategory Category { get; }
    ConsoleKey Shortcut { get; }
    string Description { get; }
    Task Execute();
}

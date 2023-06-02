namespace Time.Menu;

public interface ICommand
{
    ConsoleKey Shortcut { get; }
    virtual bool IsScreenSaverResistant => false;
    string Description { get; }
    Task Execute();
}

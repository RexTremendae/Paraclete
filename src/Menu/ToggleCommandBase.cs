namespace Paraclete.Menu;

public abstract class ToggleCommandBase(ConsoleKey shortcut, string description, bool initialState)
    : ICommand
{
    public const char FlagChar = '⚑';
    public const char UnflagChar = '⚐';

    public ConsoleKey Shortcut { get; } = shortcut;

    public string Description => $"{description} {(State ? FlagChar : UnflagChar)}";

    protected bool State { get; private set; } = initialState;

    public abstract Task Execute();

    protected virtual void Toggle()
    {
        State = !State;
    }
}

namespace Paraclete.Menu.MenuStateHandling;

public interface IMenuState<TState, TCommand>
{
    TState State { get; }
    bool IsInitialState();

    IEnumerable<(TCommand Command, bool PerformActions, TState State)> Transitions { get; }

    void ExecuteCommand(TCommand command);
    IMenuActionResult<TState> EnterAction();
    IMenuActionResult<TState> ExitAction();
}

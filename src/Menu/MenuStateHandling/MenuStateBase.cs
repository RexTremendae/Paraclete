namespace Paraclete.Menu.MenuStateHandling;

public abstract class MenuStateBase<TState, TCommand>
    : IMenuState<TState, TCommand>
    where TState : Enum
    where TCommand : Enum
{
    public virtual TState State { get; } = default!;
    public virtual Type ScreenType { get; } = typeof(object);

    public virtual bool IsInitialState()
    {
        return false;
    }

    public virtual IEnumerable<(TCommand Command, bool PerformActions, TState State)> Transitions { get; } = [];

    public virtual IMenuActionResult<TState> EnterAction() => AcceptTransition;

    public virtual void ExecuteCommand(TCommand command)
    {
    }

    public virtual IMenuActionResult<TState> ExitAction() => AcceptTransition;

    protected IMenuActionResult<TState> AcceptTransition => new AcceptTransition<TState>();
    protected IMenuActionResult<TState> RejectTransition => new RejectTransition<TState>();
    protected IMenuActionResult<TState> ForceTransition(TState state, bool performAction)
        => new ForceTransition<TState>(state, performAction);
}

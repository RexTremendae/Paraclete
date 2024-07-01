namespace Paraclete.Menu.MenuStateHandling;

using Microsoft.Extensions.DependencyInjection;

public class MenuStateMachine<TState, TCommand>
    where TState : Enum
    where TCommand : Enum
{
    private readonly Dictionary<TState, IMenuState<TState, TCommand>> _states = [];
    private readonly Dictionary<(TState State, TCommand Command), (TState State, bool PerformAction)> _transitions = [];
    private readonly NullableGeneric<TState> _initialState = NullableGeneric<TState>.CreateNullValue();

    private IMenuState<TState, TCommand> _currentState;

    public TState CurrentState => _currentState.State;

    public MenuStateMachine(IServiceProvider serviceProvider)
    {
        foreach (var type in TypeEnumerator.GetDerivedInstanceTypesOf<IMenuState<TState, TCommand>>())
        {
            var menuState = serviceProvider.GetRequiredService(type) as IMenuState<TState, TCommand>
                ?? throw new InvalidOperationException($"Could not retreive state of type {type.Name}");
            _states.Add(menuState.State, menuState);

            if (menuState.IsInitialState())
            {
                _initialState = NullableGeneric<TState>.Create(menuState.State);
            }

            foreach (var (key, performActions, state) in menuState.Transitions)
            {
                _transitions.Add((menuState.State, key), (state, performActions));
            }
        }

        if (!_initialState.HasValue)
        {
            throw new InvalidOperationException("Initial state is not defined");
        }

        _currentState = _states[_initialState.Value];
    }

    public void ExecuteCommand(TCommand command)
    {
        _currentState.ExecuteCommand(command);
        if (!_transitions.TryGetValue((_currentState.State, command), out var next))
        {
            return;
        }

        var nextStateKey = next.State;
        var performAction = next.PerformAction;
        if (next.PerformAction)
        {
            var result = _currentState.ExitAction();
            if (result is RejectTransition<TState>)
            {
                return;
            }

            if (result is ForceTransition<TState> forceResult)
            {
                nextStateKey = forceResult.State;
                performAction = forceResult.PerformAction;
            }
        }

        var nextState = _states[nextStateKey];

        if (performAction)
        {
            if (nextState.EnterAction() is RejectTransition<TState>)
            {
                return;
            }
        }

        _currentState = nextState;
    }

    public void ForceTransition(TState state)
    {
        _currentState = _states[state];
        _currentState.EnterAction();
    }

    public void Reset()
    {
        if (!_initialState.HasValue)
        {
            throw new InvalidOperationException("Initial state is not defined");
        }

        _currentState = _states[_initialState.Value];
    }
}

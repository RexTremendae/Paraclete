namespace Paraclete.Menu.MenuStateHandling;

#pragma warning disable SA1402 // File may only contain a single class
#pragma warning disable MEN008 // File name doesn't match the name of a contained type

public interface IMenuActionResult<T>
{
}

public class AcceptTransition<T> : IMenuActionResult<T>
{
}

public class RejectTransition<T> : IMenuActionResult<T>
{
}

public class ForceTransition<T>(T state, bool performAction) : IMenuActionResult<T>
{
    public T State { get; } = state;
    public bool PerformAction { get; } = performAction;
}

#pragma warning restore

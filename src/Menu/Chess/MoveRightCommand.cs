namespace Paraclete.Menu.Chess;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Screens.Chess;

public class MoveRightCommand(MenuStateMachine<ChessMenuState, ChessMenuStateCommand> stateMachine, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly MenuStateMachine<ChessMenuState, ChessMenuStateCommand> _stateMachine = stateMachine;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.RightArrow;
    public string Description => "Move right";

    public Task Execute()
    {
        _stateMachine.ExecuteCommand(ChessMenuStateCommand.Right);
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

namespace Paraclete.Menu.Chess;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Screens.Chess;

public class MoveUpCommand(MenuStateMachine<ChessMenuState, ChessMenuStateCommand> stateMachine, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly MenuStateMachine<ChessMenuState, ChessMenuStateCommand> _stateMachine = stateMachine;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Move up";

    public Task Execute()
    {
        _stateMachine.ExecuteCommand(ChessMenuStateCommand.Up);
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

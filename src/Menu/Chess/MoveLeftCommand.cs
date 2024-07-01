namespace Paraclete.Menu.Chess;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Screens.Chess;

public class MoveLeftCommand(MenuStateMachine<ChessMenuState, ChessMenuStateCommand> stateMachine, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly MenuStateMachine<ChessMenuState, ChessMenuStateCommand> _stateMachine = stateMachine;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;
    public string Description => "Move left";

    public Task Execute()
    {
        _stateMachine.ExecuteCommand(ChessMenuStateCommand.Left);
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

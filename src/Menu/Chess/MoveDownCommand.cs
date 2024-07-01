namespace Paraclete.Menu.Chess;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Screens.Chess;

public class MoveDownCommand(MenuStateMachine<ChessMenuState, ChessMenuStateCommand> stateMachine, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly MenuStateMachine<ChessMenuState, ChessMenuStateCommand> _stateMachine = stateMachine;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Move down";

    public Task Execute()
    {
        _stateMachine.ExecuteCommand(ChessMenuStateCommand.Down);
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

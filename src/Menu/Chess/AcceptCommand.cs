namespace Paraclete.Menu.Chess;

using Paraclete.Menu.MenuStateHandling;
using Paraclete.Screens.Chess;

public class AcceptCommand(MenuStateMachine<ChessMenuState, ChessMenuStateCommand> stateMachine, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly MenuStateMachine<ChessMenuState, ChessMenuStateCommand> _stateMachine = stateMachine;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.Enter;
    public string Description => "Accept";

    public Task Execute()
    {
        _stateMachine.ExecuteCommand(ChessMenuStateCommand.Accept);
        _screenInvalidator.InvalidatePane(ChessScreen.Panes.Board);
        return Task.CompletedTask;
    }
}

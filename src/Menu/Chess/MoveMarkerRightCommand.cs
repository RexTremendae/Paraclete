namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveMarkerRightCommand : ICommand
{
    private readonly PieceSelectionService _selectionService;

    public MoveMarkerRightCommand(PieceSelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public ConsoleKey Shortcut => ConsoleKey.RightArrow;
    public string Description => "Move right";

    public Task Execute()
    {
        _selectionService.MoveSelection(1, 0);
        return Task.CompletedTask;
    }
}

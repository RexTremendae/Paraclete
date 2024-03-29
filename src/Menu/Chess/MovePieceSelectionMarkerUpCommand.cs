namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MovePieceSelectionMarkerUpCommand(PieceSelectionService selectionService)
    : ICommand
{
    private readonly PieceSelectionService _selectionService = selectionService;

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Move up";

    public Task Execute()
    {
        _selectionService.MoveSelection(0, 1);
        return Task.CompletedTask;
    }
}

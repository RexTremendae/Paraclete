namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MovePieceSelectionMarkerDownCommand : ICommand
{
    private readonly PieceSelectionService _selectionService;

    public MovePieceSelectionMarkerDownCommand(PieceSelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Move down";

    public Task Execute()
    {
        _selectionService.MoveSelection(0, -1);
        return Task.CompletedTask;
    }
}

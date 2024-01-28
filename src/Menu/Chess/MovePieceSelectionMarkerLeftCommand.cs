namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MovePieceSelectionMarkerLeftCommand(PieceSelectionService selectionService)
    : ICommand
{
    private readonly PieceSelectionService _selectionService = selectionService;

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;
    public string Description => "Move left";

    public Task Execute()
    {
        _selectionService.MoveSelection(-1, 0);
        return Task.CompletedTask;
    }
}

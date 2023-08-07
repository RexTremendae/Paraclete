namespace Paraclete.Menu.Chess;

using Paraclete.Screens.Chess;

public class MoveMarkerLeftCommand : ICommand
{
    private readonly PieceSelectionService _selectionService;

    public MoveMarkerLeftCommand(PieceSelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;
    public string Description => "Move left";

    public Task Execute()
    {
        _selectionService.MoveSelection(-1, 0);
        return Task.CompletedTask;
    }
}

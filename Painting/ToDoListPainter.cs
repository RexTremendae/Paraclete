namespace Paraclete.Painting;

public class ToDoListPainter
{
    private Painter _painter;
    private ToDoList _toDoList;
    private ToDoFormatConfiguration _formatConfig;

    public ToDoListPainter(Painter painter, ToDoList toDoList)
    {
        _painter = painter;
        _toDoList = toDoList;
        _formatConfig = new (
            Header: AnsiSequences.ForegroundColors.White,
            Marker: AnsiSequences.ForegroundColors.Blue,
            ToDo: AnsiSequences.ForegroundColors.Yellow,
            Done: AnsiSequences.ForegroundColors.Gray + AnsiSequences.StrikeThrough
        );
    }

    public void Paint((int x, int y) position, bool paintSelectionMaker = false)
    {
        var toDoItemPadding = _toDoList.MaxItemLength + 3;

        var rows = new List<AnsiString>();

        rows.Add($"{_formatConfig.Header}ToDo:{AnsiSequences.Reset}{string.Empty.PadRight(toDoItemPadding)}");

        rows.AddRange(_toDoList.ToDoItems.Select(_ =>
            ResolveMarker(_, paintSelectionMaker) +
            _formatConfig.ToDo +
            _.ToDisplayString(false).PadRight(_toDoList.MaxItemLength) +
            AnsiSequences.Reset
        ));

        rows.Add(string.Empty.PadRight(toDoItemPadding));
        rows.Add($"{_formatConfig.Header}Done:{AnsiSequences.Reset}{string.Empty.PadRight(toDoItemPadding)}");

        rows.AddRange(_toDoList.DoneItems.Select(_ =>
            ResolveMarker(_, paintSelectionMaker) +
            _formatConfig.Done +
            _.ToDisplayString(true).PadRight(_toDoList.MaxItemLength) +
            AnsiSequences.Reset
        ));

        _painter.PaintRows(rows.ToArray(), position);
    }

    public string ResolveMarker(ToDoItem toDoItem, bool paintSelectionMaker)
    {
        // ToDo list without selection marker
        if (!paintSelectionMaker) return "- ";

        // Selection marker is visible but current item is not selected
        if (toDoItem != _toDoList.SelectedToDoItem) return "-  ";

        // Selected item in move item mode
        if (_toDoList.MoveItemMode) return $"{_formatConfig.Marker}⮝⮟ ";

        // Selected item in normal mode
        return $"{_formatConfig.Marker}=> ";
    }
}

public readonly record struct ToDoFormatConfiguration
(
    AnsiString Header,
    AnsiString Marker,
    AnsiString ToDo,
    AnsiString Done
);

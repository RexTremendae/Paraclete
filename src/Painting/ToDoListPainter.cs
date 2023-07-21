namespace Paraclete.Painting;

using Paraclete.Ansi;

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
            header: AnsiSequences.ForegroundColors.White,
            marker: AnsiSequences.ForegroundColors.Blue,
            toDo: AnsiSequences.ForegroundColors.Yellow,
            done: AnsiSequences.ForegroundColors.Gray + AnsiSequences.StrikeThrough
        );
    }

    public void Paint((int x, int y) position, bool paintSelectionMaker = false)
    {
        var toDoItemPadding = _toDoList.MaxItemLength + 3;

        var rows = new List<AnsiString>();

        rows.Add(_formatConfig.header + "ToDo:" + AnsiSequences.Reset + string.Empty.PadRight(toDoItemPadding));

        rows.AddRange(_toDoList.ToDoItems.Select(_ =>
            ResolveMarker(_, paintSelectionMaker) +
            _formatConfig.toDo +
            _.ToDisplayString(false).PadRight(_toDoList.MaxItemLength) +
            AnsiSequences.Reset
        ));

        rows.Add(string.Empty.PadRight(toDoItemPadding));
        rows.Add($"{_formatConfig.header}Done:{AnsiSequences.Reset}{string.Empty.PadRight(toDoItemPadding)}");

        rows.AddRange(_toDoList.DoneItems.Select(_ =>
            ResolveMarker(_, paintSelectionMaker) +
            _formatConfig.done +
            _.ToDisplayString(true).PadRight(_toDoList.MaxItemLength) +
            AnsiSequences.Reset
        ));

        _painter.PaintRows(rows, position);
    }

    public string ResolveMarker(ToDoItem toDoItem, bool paintSelectionMaker)
    {
        // ToDo list without selection marker
        if (!paintSelectionMaker)
        {
            return "- ";
        }

        // Selection marker is visible but current item is not selected
        if (toDoItem != _toDoList.SelectedToDoItem)
        {
            return "-  ";
        }

        // Selected item in move item mode
        if (_toDoList.MoveItemMode)
        {
            return $"{_formatConfig.marker}⮝⮟ ";
        }

        // Selected item in normal mode
        return $"{_formatConfig.marker}=> ";
    }
}

public readonly record struct ToDoFormatConfiguration
(
    AnsiString header,
    AnsiString marker,
    AnsiString toDo,
    AnsiString done
);

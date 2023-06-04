namespace Paraclete.Painting;

public class ToDoListPainter
{
    private Painter _painter;
    private ToDoList _toDoList;

    public ToDoListPainter(Painter painter, ToDoList toDoList)
    {
        _painter = painter;
        _toDoList = toDoList;
    }

    public void Paint((int x, int y) position, bool paintSelectionMaker = false)
    {
        var rows = new List<AnsiString>(new AnsiString[] { $"{AnsiSequences.ForegroundColors.White}ToDo:{AnsiSequences.Reset}" });

        rows.AddRange(_toDoList.ToDoItems.Select(_ =>
            ResolveMarker(_, paintSelectionMaker) +
            (_.Done ? (AnsiSequences.ForegroundColors.Gray + AnsiSequences.StrikeThrough) : AnsiSequences.ForegroundColors.Yellow) +
            _.Description.PadRight(_toDoList.MaxItemLength) +
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
        if (_toDoList.MoveItemMode) return $"{AnsiSequences.ForegroundColors.Blue}⮝⮟ ";

        // Selected item in normal mode
        return $"{AnsiSequences.ForegroundColors.Blue}=> ";
    }
}

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

    public void Paint((int x, int y) position)
    {
        var rows = new List<AnsiString>(new AnsiString[] { $"{AnsiSequences.ForegroundColors.White}ToDo:{AnsiSequences.Reset}" });

        rows.AddRange(_toDoList.ToDoItems.Select(_ =>
            "- " +
            (_.Done ? AnsiSequences.ForegroundColors.Gray : AnsiSequences.ForegroundColors.Yellow) +
            (_.Done ? AnsiSequences.StrikeThrough : "") +
            _.Description +
            AnsiSequences.Reset
        ));

        _painter.PaintRows(rows.ToArray(), position);
    }
}

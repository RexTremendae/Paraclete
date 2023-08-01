namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

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

    public void Paint(Pane pane, (int x, int y) position, bool paintSelectionMaker = false)
    {
        var toDoItemPadding = _toDoList.MaxItemLength + 3;

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.header)
            .Append("ToDo:")
            .Append(AnsiSequences.Reset)
            .Append(string.Empty.PadRight(toDoItemPadding))
            .Build()
        );

        rows.AddRange(_toDoList.ToDoItems.Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_, paintSelectionMaker))
            .Append(_formatConfig.toDo)
            .Append(_.ToDisplayString(false).PadRight(_toDoList.MaxItemLength))
            .Append(AnsiSequences.Reset)
            .Build()
        ));

        rows.Add(string.Empty.PadRight(toDoItemPadding));

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.header)
            .Append("Done:")
            .Append(AnsiSequences.Reset)
            .Append(string.Empty.PadRight(toDoItemPadding))
            .Build());

        rows.AddRange(_toDoList.DoneItems.Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_, paintSelectionMaker))
            .Append(_formatConfig.done)
            .Append(_.ToDisplayString(true).PadRight(_toDoList.MaxItemLength))
            .Append(AnsiSequences.Reset)
            .Build()
        ));

        rows.Add(string.Empty.PadRight(toDoItemPadding));

        _painter.PaintRows(rows, pane, position);
    }

    public AnsiString ResolveMarker(ToDoItem toDoItem, bool paintSelectionMaker)
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
            return _formatConfig.marker + "⮝⮟ ";
        }

        // Selected item in normal mode
        return _formatConfig.marker + "=> ";
    }
}

public readonly record struct ToDoFormatConfiguration
(
    AnsiString header,
    AnsiString marker,
    AnsiString toDo,
    AnsiString done
);

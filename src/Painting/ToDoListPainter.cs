namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

public class ToDoListPainter(Painter painter, ToDoList toDoList)
{
    private readonly Painter _painter = painter;
    private readonly ToDoList _toDoList = toDoList;
    private readonly ToDoFormatConfiguration _formatConfig = new(
        Header: AnsiSequences.ForegroundColors.White,
        Marker: AnsiSequences.ForegroundColors.Blue,
        ToDo: AnsiSequences.ForegroundColors.Yellow,
        Done: AnsiSequences.ForegroundColors.Gray + AnsiSequences.StrikeThrough
    );

    public void Paint(Pane pane, (int x, int y) position, bool paintSelectionMaker = false)
    {
        var toDoItemPadding = (pane.Size.x - position.x).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.Header)
            .Append("ToDo:")
            .Build()
        );

        rows.AddRange(_toDoList.ToDoItems.Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_, paintSelectionMaker))
            .Append(_formatConfig.ToDo)
            .Append(_.ToDisplayString(false))
            .Build()
            .PadRight(pane.Size.x - position.x)
        ));

        rows.Add(string.Empty);

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.Header)
            .Append("Done:")
            .Build()
        );

        rows.AddRange(_toDoList.DoneItems.Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_, paintSelectionMaker))
            .Append(_formatConfig.Done)
            .Append(_.ToDisplayString(true))
            .Build()
        ));

        rows.Add(string.Empty);

        _painter.PaintRows(rows, pane, position, showEllipsis: true, padPaneWidth: true);
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
            return _formatConfig.Marker + "⮝⮟ ";
        }

        // Selected item in normal mode
        return _formatConfig.Marker + "=> ";
    }
}

public readonly record struct ToDoFormatConfiguration
(
    AnsiString Header,
    AnsiString Marker,
    AnsiString ToDo,
    AnsiString Done
);

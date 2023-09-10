namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

public class NotebookPainter
{
    private readonly Painter _painter;
    private readonly Notebook _notebook;
    private readonly NotebookFormatConfiguration _formatConfig;

    public NotebookPainter(Painter painter, Notebook notebook)
    {
        _painter = painter;
        _notebook = notebook;
        _formatConfig = new (
            header: AnsiSequences.ForegroundColors.White,
            marker: AnsiSequences.ForegroundColors.Blue,
            notes: AnsiSequences.ForegroundColors.Yellow
        );
    }

    public void PaintSectionList(Pane pane, (int x, int y) position)
    {
        var itemPadding = (pane.Size.x - position.x).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.header)
            .Append("Sections")
            .Build()
            .PadRight(itemPadding)
        );

        rows.AddRange(_notebook.GetSections().Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_))
            .Append(_formatConfig.notes)
            .Append(_)
            .Build()
            .PadRight(pane.Size.x - position.x)
        ));

        _painter.PaintRows(rows, pane, position, showEllipsis: true);
    }

    public void PaintNotes(Pane pane, (int x, int y) position)
    {
        var itemPadding = (pane.Size.x - position.x).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.header)
            .Append(_notebook.SelectedSection)
            .Build()
            .PadRight(itemPadding)
        );

        rows.AddRange(_notebook.GetNotes().Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_))
            .Append(_formatConfig.notes)
            .Append(_)
            .Build()
            .PadRight(pane.Size.x - position.x)
        ));

        _painter.PaintRows(rows, pane, position, showEllipsis: true);
    }

    public AnsiString ResolveMarker(string noteItem)
    {
        return noteItem == _notebook.SelectedSection
            ? _formatConfig.marker + "=> "
            : " - ";
    }
}

public readonly record struct NotebookFormatConfiguration
(
    AnsiString header,
    AnsiString marker,
    AnsiString notes
);

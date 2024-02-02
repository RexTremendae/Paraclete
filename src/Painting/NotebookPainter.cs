namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

public class NotebookPainter(Painter painter, Notebook notebook)
{
    private readonly Painter _painter = painter;
    private readonly Notebook _notebook = notebook;
    private readonly NotebookFormatConfiguration _formatConfig = new(
        Header: AnsiSequences.ForegroundColors.White,
        Marker: AnsiSequences.ForegroundColors.Blue,
        Notes: AnsiSequences.ForegroundColors.Yellow
    );

    public void PaintSectionList(Pane pane, (int X, int Y) position)
    {
        var itemPadding = (pane.Size.X - position.X).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.Header)
            .Append("Sections")
            .Build()
            .PadRight(itemPadding)
        );

        rows.AddRange(_notebook.GetSections().Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_))
            .Append(_formatConfig.Notes)
            .Append(_)
            .Build()
        ));

        _painter.PaintRows(rows, pane, position, showEllipsis: true);
    }

    public void PaintNotes(Pane pane, (int X, int Y) position)
    {
        var itemPadding = (pane.Size.X - position.X).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        var sections = _notebook.Sections.ToArray();

        foreach (var sectionIndex in
            _notebook.SelectedSectionIndex.To(sections.Length)
            .Concat(0.To(_notebook.SelectedSectionIndex)))
        {
            var currentSection = sections[sectionIndex];

            rows.Add(builder
                .Clear()
                .Append(_formatConfig.Header)
                .Append(currentSection)
                .Build()
            );

            rows.AddRange(_notebook.GetNotes(currentSection).Select(_ => builder
                .Clear()
                .Append(ResolveMarker(_))
                .Append(_formatConfig.Notes)
                .Append(_)
                .Build()
            ));

            rows.Add(string.Empty);

            if (sectionIndex == sections.Length - 1)
            {
                rows.Add(AnsiSequences.ForegroundColors.Gray + string.Empty.PadLeft(pane.Size.X - 2, '-'));
                rows.Add(string.Empty);
            }
        }

        _painter.PaintRows(rows, pane, position, padPaneWidth: true, showEllipsis: true);
    }

    public AnsiString ResolveMarker(string noteItem)
    {
        return noteItem == _notebook.SelectedSection
            ? _formatConfig.Marker + "=> "
            : " - ";
    }
}

public readonly record struct NotebookFormatConfiguration
(
    AnsiString Header,
    AnsiString Marker,
    AnsiString Notes
);

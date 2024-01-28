namespace Paraclete.Screens;

using System;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Notes;
using Paraclete.Painting;

public class NotesScreen(NotesMenu menu, NotebookPainter notebookPainter)
    : IScreen
{
    private readonly NotebookPainter _notebookPainter = notebookPainter;

    public MenuBase Menu { get; } = menu;
    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition(20));
    public string Name => "Notes";
    public ConsoleKey Shortcut => ConsoleKey.F2;

    public Action GetPaintPaneAction(Painter painter, int paneIndex)
    {
        return paneIndex switch
        {
            0 => () =>
            {
                _notebookPainter.PaintSectionList(Layout.Panes[paneIndex], (1, 1));
            },
            1 => () =>
            {
                _notebookPainter.PaintNotes(Layout.Panes[paneIndex], (1, 1));
            },
            _ => () =>
            {
            }
        };
    }
}

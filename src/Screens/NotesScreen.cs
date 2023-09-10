namespace Paraclete.Screens;

using System;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Notes;
using Paraclete.Painting;

public class NotesScreen : IScreen
{
    private readonly NotebookPainter _notebookPainter;

    public NotesScreen(NotesMenu menu, NotebookPainter notebookPainter)
    {
        Menu = menu;
        _notebookPainter = notebookPainter;
    }

    public MenuBase Menu { get; }
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

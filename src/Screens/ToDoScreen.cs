namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

public class ToDoScreen(ToDoMenu toDoMenu, ToDoList toDoList, ToDoListPainter toDoListPainter)
    : IScreen
{
    private readonly ToDoListPainter _toDoListPainter = toDoListPainter;
    private readonly ToDoList _toDoList = toDoList;

    public string Name => "ToDo";
    public ConsoleKey Shortcut => ConsoleKey.F3;

    public MenuBase Menu { get; } = toDoMenu;
    public ILayout Layout { get; } = new SinglePaneLayout();

    public void OnAfterSwitch()
    {
        _toDoList.ResetSelection();
    }

    public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
    () =>
    {
        _toDoListPainter.Paint(Layout.Panes[0], (2, 1), true);
    };
}

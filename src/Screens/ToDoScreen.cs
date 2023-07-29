namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

public class ToDoScreen : IScreen
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly Painter _painter;
    private readonly ToDoListPainter _toDoListPainter;
    private readonly ToDoList _toDoList;

    public ToDoScreen(Stopwatch stopWatch, ToDoMenu toDoMenu, ScreenInvalidator screenInvalidator, Painter painter, ToDoList toDoList, ToDoListPainter toDoListPainter)
    {
        Menu = toDoMenu;
        _screenInvalidator = screenInvalidator;
        _painter = painter;
        _toDoListPainter = toDoListPainter;
        _toDoList = toDoList;
    }

    public string Name => "ToDo";
    public ConsoleKey Shortcut => ConsoleKey.F2;

    public MenuBase Menu { get; }
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

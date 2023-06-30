namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

public class ToDoScreen : IScreen
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly TimeWriter _currentTimeWriter;
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
        _currentTimeWriter = new TimeWriter(new ()
        {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false,
        });
    }

    public string Name => "ToDo";
    public ConsoleKey Shortcut => ConsoleKey.F2;

    public MenuBase Menu { get; }
    public ILayout Layout => new SinglePanelLayout();

    public void OnAfterSwitch()
    {
        _toDoList.ResetSelection();
    }

    public void PaintContent(Painter painter)
    {
        _toDoListPainter.Paint((2, 1), true);
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth - 7, 1), _painter);
    }
}

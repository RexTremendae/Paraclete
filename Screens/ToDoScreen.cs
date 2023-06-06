using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ToDoScreen : IScreen
{
    public MenuBase Menu { get; private set; }

    private OneFrameLayout _layout = new();
    public ILayout Layout => _layout;

    public string Name => "ToDo";

    private readonly ScreenInvalidator _screenInvalidator;
    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;
    private readonly ToDoListPainter _toDoListPainter;
    private readonly ToDoList _toDoList;

    public ToDoScreen(Stopwatch stopWatch, _ToDoMenu toDoMenu, ScreenInvalidator screenInvalidator, Painter painter, ToDoList toDoList, ToDoListPainter toDoListPainter)
    {
        Menu = toDoMenu;
        _screenInvalidator = screenInvalidator;
        _painter = painter;
        _toDoListPainter = toDoListPainter;
        _toDoList = toDoList;
        _currentTimeWriter = new TimeWriter(new() {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false
        });
    }

    public void OnAfterSwitch()
    {
        _toDoList.ResetSelection();
    }

    public void PaintContent(Painter painter)
    {
        _toDoListPainter.Paint((2, 1), true);
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

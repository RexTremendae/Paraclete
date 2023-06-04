using Paraclete.Menu;
using Paraclete.Menu.ToDo;
using Paraclete.Painting;

namespace Paraclete.Screens;

public class ToDoScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;
    public string Name => "ToDo";

    private readonly FrameInvalidator _frameInvalidator;
    private readonly TimeWriter _currentTimeWriter;
    private readonly Painter _painter;
    private readonly ToDoListPainter _toDoListPainter;
    private readonly ToDoList _toDoList;

    public ToDoScreen(Stopwatch stopWatch, _ToDoMenu toDoMenu, FrameInvalidator frameInvalidator, Painter painter, ToDoList toDoList, ToDoListPainter toDoListPainter)
    {
        _menu = toDoMenu;
        _frameInvalidator = frameInvalidator;
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

    public void PaintFrame(Painter painter, int windowWidth, int windowHeight)
    {
        var frameRows = new AnsiString[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(windowWidth-2, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == windowHeight-4)
            {
                frameRows[y] = $"╟{"".PadLeft(windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        painter.PaintRows(frameRows);
    }

    public void PaintContent(Painter painter)
    {
        _toDoListPainter.Paint((2, 1), true);
        _currentTimeWriter.Write(DateTime.Now, (Console.WindowWidth-7, 1), _painter);
    }
}

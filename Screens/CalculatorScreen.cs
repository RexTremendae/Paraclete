namespace Paraclete.Screens;

using System.Text;
using Paraclete.Calculator;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Calculator;
using Paraclete.Painting;

public class CalculatorScreen : IScreen
{
    private readonly TimeWriter _currentTimeWriter;
    private readonly CalculatorHistory _calculatorHistory;

    private ScreenInvalidator _screenInvalidator;

    public CalculatorScreen(ScreenInvalidator screenInvalidator, CalculatorMenu calculatorMenu, CalculatorHistory calculatorHistory)
    {
        _screenInvalidator = screenInvalidator;
        _calculatorHistory = calculatorHistory;

        _currentTimeWriter = new TimeWriter(new ()
        {
            FontSize = Font.Size.XS,
            Color = ConsoleColor.White,
            ShowSeconds = false,
            ShowMilliseconds = false,
        });

        Menu = calculatorMenu;
    }

    public string Name => "Calculator";
    public ConsoleKey Shortcut => ConsoleKey.F4;

    public MenuBase Menu { get; }
    public ILayout Layout => new SinglePanelLayout();

    public void PaintContent(Painter painter)
    {
        var position = (x: 2, y: 2);
        foreach (var entry in _calculatorHistory.Entries)
        {
            var builder = new StringBuilder();
            entry.AddToString(builder);

            var line =
                builder.ToString() +
                AnsiSequences.Reset +
                AnsiSequences.ForegroundColors.Gray + " = " +
                AnsiSequences.ForegroundColors.Blue + entry.Evaluate().ToString() +
                AnsiSequences.Reset;

            painter.Paint(text: line, position: position);
            position = (position.x, position.y + 1);
        }

        _currentTimeWriter.Write(DateTime.Now, (-7, 1), painter);
    }
}

namespace Paraclete.Screens;

using System.Numerics;
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
    public ILayout Layout => new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition[] { new (100) });

    public void PaintContent(Painter painter, int windowWidth, int windowHeight)
    {
        var tokenFormat = AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;

        var position = (x: 2, y: 2);
        foreach (var entry in _calculatorHistory.Entries)
        {
            var builder = new StringBuilder();

            var isFirst = true;
            foreach (var (token, _) in entry.Tokens)
            {
                if (!isFirst)
                {
                    builder.Append(" ");
                }

                builder.Append(tokenFormat + token + AnsiSequences.Reset);
                isFirst = false;
            }

            var line =
                builder.ToString() +
                AnsiSequences.Reset +
                AnsiSequences.ForegroundColors.Gray + " = " +
                AnsiSequences.ForegroundColors.Blue + entry.Evaluate().ToString() +
                AnsiSequences.Reset;

            painter.Paint(text: line, position: position);
            position = (position.x, position.y + 1);
        }

        if (_calculatorHistory.RadixConversion is BigInteger radix)
        {
            var paddingWidth = windowWidth - 108;
            var conversions = new[]
            {
                ("Dec", radix.ToString().PadRight(paddingWidth)),
                ("Bin", radix.ToBinaryString().PadRight(paddingWidth)),
                ("Hex", radix.ToHexadecimalString().PadRight(paddingWidth)),
                ("Oct", radix.ToOctalString().PadRight(paddingWidth)),
            };

            position = (103, 2);
            foreach (var (radixName, data) in conversions)
            {
                painter.Paint(AnsiSequences.ForegroundColors.White + radixName, position);
                painter.Paint(AnsiSequences.ForegroundColors.Blue + data, (position.x + 4, position.y));
                position = (position.x, position.y + 1);
            }
        }

        _currentTimeWriter.Write(DateTime.Now, (-7, 1), painter);
    }
}

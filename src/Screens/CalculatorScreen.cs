namespace Paraclete.Screens;

using System.Numerics;
using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Calculator;
using Paraclete.Modules.Calculator;
using Paraclete.Painting;

public class CalculatorScreen(CalculatorMenu calculatorMenu, CalculatorHistory calculatorHistory) : IScreen
{
    private readonly CalculatorHistory _calculatorHistory = calculatorHistory;

    public string Name => "Calculator";

    public ConsoleKey Shortcut => ConsoleKey.F4;

    public MenuBase Menu { get; } = calculatorMenu;
    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition(100));

    public static class Panes
    {
        public const int Calculations = 0;
        public const int Radix = 1;
    }

    public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
        paneIndex switch
        {
            0 => () => PaintExpressions(painter, Layout.Panes[paneIndex]),
            1 => () => PaintRadixConversion(painter, Layout.Panes[paneIndex]),
            _ => () => { },
        };

    private void PaintExpressions(Painter painter, Pane pane)
    {
        var tokenFormat = AnsiSequences.BackgroundColors.DarkBlue + AnsiSequences.ForegroundColors.Blue;

        var position = (x: 1, y: 2);
        foreach (var entry in _calculatorHistory.Entries)
        {
            var builder = new AnsiStringBuilder();

            var isFirst = true;
            foreach (var (token, _) in entry.Tokens)
            {
                if (!isFirst)
                {
                    builder.Append(" ");
                }

                builder.Append(tokenFormat).Append(token).Append(AnsiSequences.Reset);
                isFirst = false;
            }

            builder
                .Append(AnsiSequences.Reset)
                .Append(AnsiSequences.ForegroundColors.Gray)
                .Append(" = ")
                .Append(AnsiSequences.ForegroundColors.Blue)
                .Append(entry.Evaluate().ToString());

            painter.Paint(builder.Build(), pane, position);
            position = (position.x, position.y + 1);
        }
    }

    private void PaintRadixConversion(Painter painter, Pane pane)
    {
        if (_calculatorHistory.RadixConversion is BigInteger radix)
        {
            var offsetX = 1;
            var offsetY = 2;

            var paddingWidth = (pane.Size.X - offsetX - 4).ZeroFloor();
            var conversions = new[]
            {
                ("Dec", radix.ToDecimalString(useGrouping: true, padGroups: false).PadRight(paddingWidth)),
                ("Bin", radix.ToBinaryString(useGrouping: true, padGroups: true).PadRight(paddingWidth)),
                ("Hex", radix.ToHexadecimalString(useGrouping: true, padGroups: true).PadRight(paddingWidth)),
                ("Oct", radix.ToOctalString(useGrouping: true, padGroups: true).PadRight(paddingWidth)),
            };

            var position = (x: offsetX, y: offsetY);
            foreach (var (radixName, data) in conversions)
            {
                painter.Paint(AnsiSequences.ForegroundColors.White + radixName, pane, position);
                painter.Paint(AnsiSequences.ForegroundColors.Blue + data, pane, (position.x + radixName.Length + 1, position.y));
                position = (position.x, position.y + 1);
            }
        }
    }
}

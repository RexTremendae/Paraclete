namespace Paraclete.Screens.Unicode;

using System.Numerics;
using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Unicode;
using Paraclete.Painting;
using Paraclete.Screens.Unicode;

public class UnicodeScreen : IScreen
{
    private readonly MenuBase _menu;
    private readonly ILayout _layout;
    private readonly UnicodeControl _unicodeControl;

    public UnicodeScreen(UnicodeMenu menu, UnicodeControl unicodeControl)
    {
        _menu = menu;
        _layout = new SinglePaneLayout();
        _unicodeControl = unicodeControl;
    }

    public MenuBase Menu => _menu;
    public ILayout Layout => _layout;

    public string Name => "Unicode";

    public ConsoleKey Shortcut => ConsoleKey.F5;

    public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
    () =>
    {
        var count = Layout.Panes[paneIndex].Size.y;
        var rows = new List<AnsiStringBuilder>(Enumerable
            .Range(0, count)
            .Select(_ => new AnsiStringBuilder()));

        for (var column = 0; column < _unicodeControl.ColumnStartValues.Length; column++)
        {
            for (int i = 0; i < count; i++)
            {
                var startMarker = (i == 0 && _unicodeControl.SelectedColumn == column) ? "[" : " ";
                var endMarker = (i == 0 && _unicodeControl.SelectedColumn == column) ? "]" : " ";

                var codepoint = _unicodeControl.ColumnStartValues[column] + i;
                rows[i].Append(
                    AnsiSequences.ForegroundColors.White +
                    $"{startMarker}" +
                    AnsiSequences.ForegroundColors.Gray +
                    $"0x{((BigInteger)codepoint).ToHexadecimalString().PadLeft(4, '0')} " +
                    AnsiSequences.ForegroundColors.Cyan +
                    $"{(char)(codepoint)}" +
                    AnsiSequences.ForegroundColors.White +
                    $"{endMarker}" +
                    AnsiSequences.ForegroundColors.DarkGray +
                    $"│ ");
            }
        }

        painter.PaintRows(rows.Select(_ => _.Build()), Layout.Panes[0]);
    };
}
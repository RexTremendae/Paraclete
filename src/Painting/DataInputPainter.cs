namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.IO;

public class DataInputPainter(Settings settings, DataInputter dataInputter)
{
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly Settings _settings = settings;

    public (int cursorX, int cursorY) PaintInput(Painter painter, int windowWidth, int windowHeight)
    {
        var dataInputColor = string.IsNullOrWhiteSpace(_dataInputter.ErrorMessage)
            ? _settings.Colors.InputData
            : _settings.Colors.ErroneousInputData;

        var errorTextLength = windowWidth - _dataInputter.Label.Length - 4;
        var errorText = (_settings.Colors.ErroneousInputData + _dataInputter.ErrorMessage).PadRight(errorTextLength);
        var description = " " + _settings.Colors.InputLabel + _dataInputter.Label + " " + errorText;

        var cursorText = string.Empty.PadRight((windowWidth - _dataInputter.CurrentInput.Length - 3).ZeroFloor());
        var input = " " + dataInputColor + _dataInputter.CurrentInput + _settings.Colors.InputLabel + cursorText;

        var rows = new AnsiString[]
        {
            description,
            input,
        };

        painter.PaintRows(rows, (1, -3), (windowWidth - 1, windowHeight - 1));
        var cursorX = _dataInputter.CurrentInput.Length + 2;
        var cursorY = windowHeight - 2;

        return (cursorX, cursorY);
    }
}

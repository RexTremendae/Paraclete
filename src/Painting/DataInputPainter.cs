namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.IO;

public class DataInputPainter
{
    private readonly DataInputter _dataInputter;
    private readonly Settings _settings;

    public DataInputPainter(Settings settings, DataInputter dataInputter)
    {
        _dataInputter = dataInputter;
        _settings = settings;
    }

    public void PaintInput(Painter painter, int windowWidth, int windowHeight)
    {
        var dataInputColor = string.IsNullOrWhiteSpace(_dataInputter.ErrorMessage)
            ? _settings.Colors.InputData
            : _settings.Colors.ErroneousInputData;

        var errorTextLength = windowWidth - _dataInputter.Label.Length - 5;
        var errorText = _settings.Colors.ErroneousInputData + _dataInputter.ErrorMessage.PadRight(errorTextLength);
        var cursorText = "▂".PadRight(windowWidth - _dataInputter.CurrentInput.Length - 5);

        var rows = new AnsiString[]
        {
            _settings.Colors.InputLabel + _dataInputter.Label + " " + errorText,
            dataInputColor + _dataInputter.CurrentInput + _settings.Colors.InputLabel + cursorText,
        };

        painter.PaintRows(rows, (2, -3));
    }
}

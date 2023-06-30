namespace Paraclete.Painting;

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
        var rows = new AnsiString[] {
            _settings.Colors.InputLabel + _dataInputter.Label,
            _settings.Colors.InputData + _dataInputter.CurrentInput
            + _settings.Colors.InputLabel + "▂".PadRight(windowWidth - _dataInputter.CurrentInput.Length - 5),
        };

        painter.PaintRows(rows, (2, -3));
    }
}

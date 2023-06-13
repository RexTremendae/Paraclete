using Paraclete.Menu;

namespace Paraclete.Painting;

public class DataInputPainter
{
    private readonly IServiceProvider _services;
    private readonly DataInputter _dataInputter;

    public DataInputPainter(IServiceProvider services, DataInputter dataInputter)
    {
        _services = services;
        _dataInputter = dataInputter;
    }

    public void PaintInput(Painter painter, int windowWidth, int windowHeight)
    {
        var rows = new AnsiString[] {
            AnsiSequences.ForegroundColors.White + _dataInputter.Label,
            AnsiSequences.ForegroundColors.Yellow + _dataInputter.CurrentInput + AnsiSequences.ForegroundColors.White + "▂".PadRight(windowWidth - _dataInputter.CurrentInput.Length - 5)
        };

        painter.PaintRows(rows, (2, -3));
    }
}

namespace Paraclete;

using Paraclete.Ansi;

public class FpsCounter(Settings settings)
{
    private readonly List<int> _fpsHistory = [];
    private readonly int _maxFpsHistory = settings.FpsCounter.HistoryLength;
    private readonly bool _isEnabled = settings.FpsCounter.IsEnabled;

    private int _frameCount;
    private DateTime _frameCountStart;
    private double _fpsAverage;

    public void Update()
    {
        if (!_isEnabled)
        {
            return;
        }

        _frameCount++;
        var now = DateTime.Now;
        if (now - _frameCountStart > TimeSpan.FromSeconds(1))
        {
            _fpsHistory.Add(_frameCount);
            if (_fpsHistory.Count > _maxFpsHistory)
            {
                _fpsHistory.RemoveAt(0);
            }

            _fpsAverage = ((double)_fpsHistory.Sum()) / _fpsHistory.Count;

            _frameCount = 0;
            _frameCountStart = now;
        }
    }

    public void Print()
    {
        if (!_isEnabled)
        {
            return;
        }

        Console.SetCursorPosition(1, 1);
        Console.Write(
            AnsiSequences.BackgroundColor(120, 60, 120) +
            AnsiSequences.ForegroundColor(0, 60, 120) +
            $"{_fpsHistory.Last()} ({_fpsAverage:0.00})".Replace(",", ".") +
            AnsiSequences.Reset);
    }
}

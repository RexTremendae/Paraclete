namespace Paraclete;

public class Stopwatch
{
    private readonly List<TimeSpan> _markedTimes = new ();

    public DateTime Start { get; private set; }
    public DateTime Stop { get; private set; }

    public bool IsRunning { get; private set; }
    public TimeSpan[] MarkedTimes => _markedTimes.ToArray();

    public void Reset()
    {
        Start = IsRunning
            ? DateTime.UtcNow
            : default;
        Stop = default;
        _markedTimes.Clear();
    }

    public void Mark()
    {
        var mark = (IsRunning ? DateTime.UtcNow : Stop) - Start;
        if (_markedTimes.FirstOrDefault() != mark)
        {
            _markedTimes.Insert(0, mark);
        }
    }

    public void ToggleStartStop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            Stop = DateTime.UtcNow;
        }
        else
        {
            IsRunning = true;
            if (Start == default)
            {
                Start = DateTime.UtcNow;
            }
            else
            {
                Start += (DateTime.UtcNow - Stop);
            }
        }
    }
}

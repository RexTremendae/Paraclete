namespace Time;

public class Stopwatch
{
    public DateTime Start { get; private set; }
    public DateTime Stop { get; private set; }

    private readonly List<TimeSpan> _markedTimes = new();

    public bool IsRunning { get; private set; }
    public TimeSpan[] MarkedTimes => _markedTimes.ToArray();

    public void Reset()
    {
        Start = IsRunning
            ? DateTime.Now
            : default;
        Stop = default;
        _markedTimes.Clear();
    }

    public void Mark()
    {
        var mark = (IsRunning ? DateTime.Now : Stop) - Start;
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
            Stop = DateTime.Now;
        }
        else
        {
            IsRunning = true;
            if (Start == default)
            {
                Start = DateTime.Now;
            }
            else
            {
                Start += (DateTime.Now - Stop);
            }
        }
    }
}

namespace Time;

public readonly record struct TimeWriterSettings
{
    public TimeWriterSettings()
    {
        Color = ConsoleColor.Gray;
        SecondsColor = ConsoleColor.Gray;
        MillisecondsColor = ConsoleColor.DarkGray;
        ShowMilliseconds = false;
        ShowSeconds = true;
        ShowHours = true;
        FontSize = 2;
        DigitDisplayChar = '█';
    }

    public ConsoleColor Color { get; init; }
    public ConsoleColor MillisecondsColor { get; init; }
    public ConsoleColor SecondsColor { get; init; }
    public bool ShowMilliseconds { get; init; }
    public bool ShowSeconds { get; init; }
    public bool ShowHours { get; init; }
    public int FontSize { get; init; }
    public char DigitDisplayChar { get; init; }
}
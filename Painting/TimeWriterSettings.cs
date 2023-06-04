namespace Paraclete.Painting;

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
        FontSize = Font.Size.M;
        SecondsFontSize = Font.Size.Undefined;
        MillisecondsFontSize = Font.Size.Undefined;
    }

    public ConsoleColor Color { get; init; }
    public ConsoleColor MillisecondsColor { get; init; }
    public ConsoleColor SecondsColor { get; init; }
    public bool ShowMilliseconds { get; init; }
    public bool ShowSeconds { get; init; }
    public bool ShowHours { get; init; }
    public Font.Size FontSize { get; init; }
    public Font.Size SecondsFontSize { get; init; }
    public Font.Size MillisecondsFontSize { get; init; }
}

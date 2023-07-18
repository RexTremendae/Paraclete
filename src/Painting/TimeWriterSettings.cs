namespace Paraclete.Painting;

using Paraclete.Ansi;

public readonly record struct TimeWriterSettings
{
    public TimeWriterSettings()
    {
        Color = AnsiSequences.ForegroundColors.Gray;
        SecondsColor = AnsiSequences.ForegroundColors.Gray;
        MillisecondsColor = AnsiSequences.ForegroundColors.DarkGray;
        ShowMilliseconds = false;
        ShowSeconds = true;
        ShowHours = true;
        FontSize = Font.Size.M;
        SecondsFontSize = Font.Size.Undefined;
        MillisecondsFontSize = Font.Size.Undefined;
    }

    public AnsiControlSequence Color { get; init; }
    public AnsiControlSequence MillisecondsColor { get; init; }
    public AnsiControlSequence SecondsColor { get; init; }
    public bool ShowMilliseconds { get; init; }
    public bool ShowSeconds { get; init; }
    public bool ShowHours { get; init; }
    public Font.Size FontSize { get; init; }
    public Font.Size SecondsFontSize { get; init; }
    public Font.Size MillisecondsFontSize { get; init; }
}

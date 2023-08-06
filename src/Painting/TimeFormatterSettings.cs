namespace Paraclete.Painting;

using Paraclete.Ansi;

public readonly record struct TimeFormatterSettings
{
    public TimeFormatterSettings()
    {
        Color = AnsiSequences.ForegroundColors.Gray;
        SecondsColor = AnsiSequences.ForegroundColors.Gray;
        MillisecondsColor = AnsiSequences.ForegroundColors.DarkGray;
        DateColor = AnsiSequences.ForegroundColors.Gray;
        ShowMilliseconds = false;
        ShowSeconds = true;
        ShowHours = true;
        ShowDate = false;
        FontSize = Font.Size.M;
        SecondsFontSize = Font.Size.Undefined;
        MillisecondsFontSize = Font.Size.Undefined;
    }

    public AnsiControlSequence Color { get; init; }
    public AnsiControlSequence MillisecondsColor { get; init; }
    public AnsiControlSequence SecondsColor { get; init; }
    public AnsiControlSequence DateColor { get; init; }
    public bool ShowMilliseconds { get; init; }
    public bool ShowSeconds { get; init; }
    public bool ShowHours { get; init; }
    public bool ShowDate { get; init; }
    public Font.Size FontSize { get; init; }
    public Font.Size SecondsFontSize { get; init; }
    public Font.Size MillisecondsFontSize { get; init; }
}

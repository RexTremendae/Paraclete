namespace Paraclete.Extensions;

public static class ConsoleColorExtensions
{
    public static AnsiString ToAnsiForegroundColorCode(this ConsoleColor color) => color switch
    {
        ConsoleColor.Black       => AnsiSequences.ForegroundColors.Black,
        ConsoleColor.DarkRed     => AnsiSequences.ForegroundColors.DarkRed,
        ConsoleColor.DarkGreen   => AnsiSequences.ForegroundColors.DarkGreen,
        ConsoleColor.DarkYellow  => AnsiSequences.ForegroundColors.DarkYellow,
        ConsoleColor.DarkBlue    => AnsiSequences.ForegroundColors.DarkBlue,
        ConsoleColor.DarkMagenta => AnsiSequences.ForegroundColors.DarkMagenta,
        ConsoleColor.DarkCyan    => AnsiSequences.ForegroundColors.DarkCyan,
        ConsoleColor.Gray        => AnsiSequences.ForegroundColors.Gray,
        ConsoleColor.DarkGray    => AnsiSequences.ForegroundColors.DarkGray,
        ConsoleColor.Red         => AnsiSequences.ForegroundColors.Red,
        ConsoleColor.Green       => AnsiSequences.ForegroundColors.Green,
        ConsoleColor.Yellow      => AnsiSequences.ForegroundColors.Yellow,
        ConsoleColor.Blue        => AnsiSequences.ForegroundColors.Blue,
        ConsoleColor.Magenta     => AnsiSequences.ForegroundColors.Magenta,
        ConsoleColor.Cyan        => AnsiSequences.ForegroundColors.Cyan,
        ConsoleColor.White       => AnsiSequences.ForegroundColors.White,
        _ => throw new ArgumentException(message: $"Undefined color: {color}", paramName: nameof(color))
    };

    public static AnsiString ToAnsiBackgroundColorCode(this ConsoleColor color) => color switch
    {
        ConsoleColor.Black       => AnsiSequences.BackgroundColors.Black,
        ConsoleColor.DarkRed     => AnsiSequences.BackgroundColors.DarkRed,
        ConsoleColor.DarkGreen   => AnsiSequences.BackgroundColors.DarkGreen,
        ConsoleColor.DarkYellow  => AnsiSequences.BackgroundColors.DarkYellow,
        ConsoleColor.DarkBlue    => AnsiSequences.BackgroundColors.DarkBlue,
        ConsoleColor.DarkMagenta => AnsiSequences.BackgroundColors.DarkMagenta,
        ConsoleColor.DarkCyan    => AnsiSequences.BackgroundColors.DarkCyan,
        ConsoleColor.Gray        => AnsiSequences.BackgroundColors.Gray,
        ConsoleColor.DarkGray    => AnsiSequences.BackgroundColors.DarkGray,
        ConsoleColor.Red         => AnsiSequences.BackgroundColors.Red,
        ConsoleColor.Green       => AnsiSequences.BackgroundColors.Green,
        ConsoleColor.Yellow      => AnsiSequences.BackgroundColors.Yellow,
        ConsoleColor.Blue        => AnsiSequences.BackgroundColors.Blue,
        ConsoleColor.Magenta     => AnsiSequences.BackgroundColors.Magenta,
        ConsoleColor.Cyan        => AnsiSequences.BackgroundColors.Cyan,
        ConsoleColor.White       => AnsiSequences.BackgroundColors.White,
        _ => throw new ArgumentException(message: $"Undefined color: {color}", paramName: nameof(color))
    };
}

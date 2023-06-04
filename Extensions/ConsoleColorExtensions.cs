namespace Time.Extensions;

public static class ConsoleColorExtensions
{
    public static AnsiString ToAnsiColorCode(this ConsoleColor color) => color switch
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
}

namespace Paraclete.Extensions;

public static class ConsoleKeyExtensions
{
    public static string ToDisplayString(this ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.Escape => "ESC",
            ConsoleKey.UpArrow => "⮝",
            ConsoleKey.DownArrow => "⮟",
            ConsoleKey.LeftArrow => "⮜",
            ConsoleKey.RightArrow => "⮞",
            ConsoleKey.Enter => "↩",
            _ => key.ToString(),
        };
    }
}

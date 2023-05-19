namespace Time;

public class Settings
{
    public int UpdateInterval { get; set; } = 50;
    public ConsoleColor Color { get; set; } = ConsoleColor.Gray;
    public ConsoleColor MillisecondsColor { get; set; } = ConsoleColor.DarkGray;
    public bool ShowMilliseconds { get; set; } = true;
    public int FontSize { get; set; } = 2;
    public char DigitDisplayChar = '#';
}

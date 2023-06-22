namespace Paraclete.Extensions;

public static class AnsiStringExtensions
{
    public static AnsiString PadRight(this AnsiString stringToPad, int totalWidth, char paddingChar = ' ')
    {
        var paddingWidth = int.Max(0, totalWidth - stringToPad.Length);
        return stringToPad + "".PadRight(paddingWidth, paddingChar);
    }
}
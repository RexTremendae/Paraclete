namespace Paraclete.Extensions;

using Paraclete.Ansi;

public static class AnsiStringExtensions
{
    public static AnsiString PadRight(this AnsiString stringToPad, int totalWidth, char paddingChar = ' ')
    {
        var paddingWidth = int.Max(0, totalWidth - stringToPad.Length);
        return stringToPad + string.Empty.PadRight(paddingWidth, paddingChar);
    }
}
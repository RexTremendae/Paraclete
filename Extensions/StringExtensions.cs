namespace Paraclete.Extensions;

public static class StringExtensions
{
    public static string RemoveBeginning(this string text, string beginning)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return text.StartsWith(beginning) ? text[beginning.Length..] : text;
    }

    public static string RemoveEnding(this string text, string ending)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return text.EndsWith(ending) ? text[..^ending.Length] : text;
    }
}

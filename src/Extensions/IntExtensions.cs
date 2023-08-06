namespace Paraclete.Extensions;

public static class IntExtensions
{
    public static IEnumerable<int> To(this int start, int end, bool endIsInclusive = false)
    {
        return (end >= start)
            ? Enumerable.Range(start, end - start + (endIsInclusive ? 1 : 0))
            : throw new ArgumentException(message: $"End ({end}) cannot be smaller than ({start})", paramName: nameof(end));
    }
}

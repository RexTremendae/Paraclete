namespace Time;

public class AnsiString
{
    private const string AnsiSequenceStart = "\u001b[";
    private readonly string _content;

    private AnsiString(string content)
    {
        _content = content;
        Length = CalculateLength(content);
    }

    private AnsiString(string content, int length)
    {
        _content = content;
        Length = length;
    }

    public int Length { get; private set; }

    public static AnsiString Empty => new AnsiString(string.Empty);

    private int CalculateLength(string data)
    {
        var length = 0;

        var lastIdx = 0;
        for(;;)
        {
            var nextIdx = data.IndexOf(AnsiSequenceStart, lastIdx);
            if (nextIdx < 0)
            {
                length += data.Length-lastIdx;
                break;
            }
            length += nextIdx-lastIdx-1;

            nextIdx = data.IndexOf("m", lastIdx);
            if (nextIdx < 0)
            {
                length += data.Length-lastIdx;
                break;
            }

            lastIdx = nextIdx+1;
        }

        return length;
    }

    public static AnsiString operator + (AnsiString as1, AnsiString as2)
    {
        return new AnsiString(as1._content + as2._content, as1.Length + as2.Length);
    }

    public static AnsiString Create(string input) => new AnsiString(input);
    public override string ToString() => _content;
    public static implicit operator AnsiString (string input) => new AnsiString(input);
}

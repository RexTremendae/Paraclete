namespace Paraclete.Ansi;

using System.Text.RegularExpressions;

public class AnsiControlSequence
{
    private static readonly Regex _validationRegex = new (
        $@"^{AnsiSequences.EscapeCharacter}\[\??(\d+(;\d+)*)?[a-zA-Z]$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(500));

    private readonly string _sequence;

    public AnsiControlSequence(string sequence)
    {
        if (!_validationRegex.IsMatch(sequence) && sequence != $"{AnsiSequences.EscapeCharacter}c")
        {
            var printableSequence = sequence.Replace(AnsiSequences.EscapeCharacter.ToString(), "\\u001b");
            throw new ArgumentException($"Invalid control sequence: '{printableSequence}'", paramName: nameof(sequence));
        }

        _sequence = sequence;
    }

    public static implicit operator AnsiString(AnsiControlSequence input) => new (input);

    public static AnsiString operator +(AnsiControlSequence sequence1, AnsiControlSequence sequence2)
    {
        return new AnsiString(sequence1, sequence2);
    }

    public static AnsiString operator +(string text, AnsiControlSequence sequence)
    {
        return new AnsiStringBuilder()
            .Append(text)
            .Append(sequence)
            .Build();
    }

    public static AnsiString operator +(AnsiControlSequence sequence, string text)
    {
        return new AnsiStringBuilder()
            .Append(sequence)
            .Append(text)
            .Build();
    }

    public override string ToString() => _sequence;
}

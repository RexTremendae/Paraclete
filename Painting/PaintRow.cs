namespace Paraclete.Painting;

public readonly record struct PaintRow
(
    PaintSection[] sections
);

public readonly record struct PaintSection
(
    string text,
    ConsoleColor? foregroundColor,
    ConsoleColor? backgroundColor
);

public class AnsiStringBuilder
{
    private List<PaintSection> _sections = new ();

    public void Append(PaintSection section) => _sections.Add(section);

    public void Append((string text, ConsoleColor foregroundColor) section)
        => _sections.Add(new (section.text, foregroundColor: section.foregroundColor, backgroundColor: null));

    public void Append((string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor) section)
        => _sections.Add(new (section.text, foregroundColor: section.foregroundColor, backgroundColor: section.backgroundColor));

    public void Append(IEnumerable<PaintSection> sections) => _sections.AddRange(sections);
    public void Append(PaintRow paintRow) => _sections.AddRange(paintRow.sections);
    public void Clear() => _sections.Clear();

    public AnsiString Build() => string.Concat(_sections.Select(_ =>
        (_.foregroundColor?.ToAnsiForegroundColorCode() ?? string.Empty) +
        (_.backgroundColor?.ToAnsiBackgroundColorCode() ?? string.Empty) +
        _.text));
}

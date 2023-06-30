namespace Paraclete.Painting;

public readonly record struct PaintRow
(
    PaintSection[] Sections
);

public readonly record struct PaintSection
(
    string Text,
    ConsoleColor? ForegroundColor,
    ConsoleColor? BackgroundColor
);

public class AnsiStringBuilder
{
    private List<PaintSection> _sections = new ();

    public void Append(PaintSection section) => _sections.Add(section);

    public void Append((string text, ConsoleColor foregroundColor) section)
        => _sections.Add(new (section.text, ForegroundColor: section.foregroundColor, BackgroundColor: null));

    public void Append((string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor) section)
        => _sections.Add(new (section.text, ForegroundColor: section.foregroundColor, BackgroundColor: section.backgroundColor));

    public void Append(IEnumerable<PaintSection> sections) => _sections.AddRange(sections);
    public void Append(PaintRow paintRow) => _sections.AddRange(paintRow.Sections);
    public void Clear() => _sections.Clear();

    public AnsiString Build() => string.Concat(_sections.Select(_ =>
        (_.ForegroundColor?.ToAnsiForegroundColorCode() ?? string.Empty) +
        (_.BackgroundColor?.ToAnsiBackgroundColorCode() ?? string.Empty) +
        _.Text));
}

namespace Paraclete.Painting;

public readonly record struct PaintRow
(
    PaintSection[] Sections
);

public readonly record struct PaintSection
(
    string Text,
    ConsoleColor Color
);

public class PaintRowBuilder
{
    private List<PaintSection> _sections = new();

    public void Append(PaintSection section) => _sections.Add(section);
    public void Append((string text, ConsoleColor color) section) => _sections.Add(new (section.text, section.color));
    public void Clear() => _sections.Clear();
    public PaintRow Build() => new PaintRow(_sections.ToArray());
}

namespace Paraclete.Painting;

using System.Collections.Concurrent;

public partial class Font
{
    private static readonly ConcurrentDictionary<Size, Font> _fontsBySize = new ();
    private readonly Dictionary<char, string[]> _font;

    public Font(Size size, params (char key, string[] data)[] font)
    {
        _font = [];

        CharacterWidth = 0;
        CharacterHeight = 0;

        foreach (var (key, data) in font)
        {
            if (CharacterHeight == 0)
            {
                CharacterHeight = data.Length;
            }
            else if (data.Length != CharacterHeight)
            {
                throw new InvalidOperationException($"Character '{key}' of font size '{size}' has an inconsistent height.");
            }

            foreach (var rowLength in data.Select(_ => _.Length))
            {
                if (CharacterWidth == 0)
                {
                    CharacterWidth = rowLength;
                }
                else if (rowLength != CharacterWidth)
                {
                    throw new InvalidOperationException($"Character '{key}' of font size '{size}' has an inconsistent width.");
                }
            }

            _font.Add(key, data);
        }
    }

    public enum Size
    {
        Undefined = 0,
        XS,
        S,
        M,
        L,
    }

    public int CharacterWidth { get; }
    public int CharacterHeight { get; }

    public string[] this[char index] => _font[index];

    public static Font OfSize(Size size)
    {
        if (_fontsBySize.TryGetValue(size, out var font))
        {
            return font;
        }

        font = GetFontDefinition(size);
        _fontsBySize[size] = font;
        return font;
    }

    private static Font GetFontDefinition(Size size) => size switch
    {
        Size.XS => _fontDefinitionXS,
        Size.S  => _fontDefinitionS,
        Size.M  => _fontDefinitionM,
        Size.L  => _fontDefinitionL,
        _ => throw new InvalidOperationException($"Undefined size: {size}")
    };
}

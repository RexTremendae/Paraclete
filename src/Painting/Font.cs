namespace Paraclete.Painting;

using System.Collections.Concurrent;

public class Font
{
    private static readonly ConcurrentDictionary<Size, Font> _fontsBySize = new ();
    private readonly Dictionary<char, string[]> _font = new ();

    public Font(Size size, params (char key, string[] data)[] font)
    {
        _font = new ();

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

            foreach (var row in data)
            {
                if (CharacterWidth == 0)
                {
                    CharacterWidth = row.Length;
                }
                else if (row.Length != CharacterWidth)
                {
                    throw new InvalidOperationException($"Character '{key}' of font size '{size}' has an inconsistent width.");
                }
            }

            _font.Add(key, data);
        }
    }

    private Font(int characterWidth, int characterHeight)
    {
        CharacterHeight = characterHeight;
        CharacterWidth = characterWidth;
        _font = new ();
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

    public string[] this[char index]
    {
        get => _font[index];
    }

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

#pragma warning disable SA1500 // Braces for multi-line statements should not share line

    private static Font GetFontDefinition(Size size) => size switch
    {
        Size.XS => new (Size.XS,
            ('0', new[] { "0" }),
            ('1', new[] { "1" }),
            ('2', new[] { "2" }),
            ('3', new[] { "3" }),
            ('4', new[] { "4" }),
            ('5', new[] { "5" }),
            ('6', new[] { "6" }),
            ('7', new[] { "7" }),
            ('8', new[] { "8" }),
            ('9', new[] { "9" }),
            (':', new[] { ":" }),
            ('.', new[] { "." })
        ),

        Size.S => new (Size.S,
            ('0', new[] { " ▄  ",
                          "█ █ ",
                          "▀▄▀ ", }),

            ('1', new[] { " ▄  ",
                          "▀█  ",
                          "▄█▄ ", }),

            ('2', new[] { " ▄  ",
                          "▀ █ ",
                          "▄█▄ ", }),

            ('3', new[] { "▄▄  ",
                          " ▄▀ ",
                          "▄▄▀ ", }),

            ('4', new[] { " ▄▄ ",
                          "█▄█ ",
                          "  █ ", }),

            ('5', new[] { "▄▄▄ ",
                          "█▄  ",
                          "▄▄▀ ", }),

            ('6', new[] { " ▄▄ ",
                          "█▄  ",
                          "▀▄▀ ", }),

            ('7', new[] { "▄▄▄ ",
                          " ▄▀ ",
                          "█   ", }),

            ('8', new[] { " ▄  ",
                          "▀▄▀ ",
                          "▀▄▀ ", }),

            ('9', new[] { " ▄  ",
                          "▀▄█ ",
                          "▄▄▀ ", }),

            (':', new[] { "    ",
                          " ▀  ",
                          " ▀  ", }),

            ('.', new[] { "    ",
                          "    ",
                          " ▄  ", })
        ),

        Size.M => new (Size.M,
            ('0', new[] { " ██  ",
                          "█  █ ",
                          "█  █ ",
                          "█  █ ",
                          " ██  ", }),

            ('1', new[] { "  █  ",
                          " ██  ",
                          "  █  ",
                          "  █  ",
                          " ███ ", }),

            ('2', new[] { " ██  ",
                          "█  █ ",
                          "  █  ",
                          " █   ",
                          "████ ", }),

            ('3', new[] { " ██  ",
                          "█  █ ",
                          "  █  ",
                          "█  █ ",
                          " ██  ", }),

            ('4', new[] { "  ██ ",
                          " █ █ ",
                          "█  █ ",
                          "████ ",
                          "   █ ", }),

            ('5', new[] { "████ ",
                          "█    ",
                          "███  ",
                          "   █ ",
                          "███  ", }),

            ('6', new[] { " ██  ",
                          "█    ",
                          "███  ",
                          "█  █ ",
                          " ██  ", }),

            ('7', new[] { "████ ",
                          "█  █ ",
                          "  █  ",
                          "  █  ",
                          "  █  ", }),

            ('8', new[] { " ██  ",
                          "█  █ ",
                          " ██  ",
                          "█  █ ",
                          " ██  ", }),

            ('9', new[] { " ██  ",
                          "█  █ ",
                          " ███ ",
                          "   █ ",
                          " ██  ", }),

            (':', new[] { "     ",
                          "  █  ",
                          "     ",
                          "  █  ",
                          "     ", }),

            ('.', new[] { "     ",
                          "     ",
                          "     ",
                          "     ",
                          "  █  ", })
        ),

        Size.L => new (Size.L,
            ('0', new[] { " ████  ",
                          "██  ██ ",
                          "██  ██ ",
                          "██  ██ ",
                          "██  ██ ",
                          "██  ██ ",
                          " ████  ", }),

            ('1', new[] { "  ██   ",
                          " ███   ",
                          "  ██   ",
                          "  ██   ",
                          "  ██   ",
                          "  ██   ",
                          " ████  ", }),

            ('2', new[] { " ████  ",
                          "██  ██ ",
                          "    ██ ",
                          "  ███  ",
                          " ██    ",
                          "██  ██ ",
                          "██████ ", }),

            ('3', new[] { " ████  ",
                          "██  ██ ",
                          "    ██ ",
                          "  ███  ",
                          "    ██ ",
                          "██  ██ ",
                          " ████  ", }),

            ('4', new[] { "   ██  ",
                          "  ███  ",
                          " █ ██  ",
                          "█  ██  ",
                          "██████ ",
                          "   ██  ",
                          "  ████ ", }),

            ('5', new[] { "██████ ",
                          "██  ██ ",
                          "██     ",
                          "█████  ",
                          "    ██ ",
                          "██  ██ ",
                          " ████  ", }),

            ('6', new[] { " ████  ",
                          "██  ██ ",
                          "██     ",
                          "█████  ",
                          "██  ██ ",
                          "██  ██ ",
                          " ████  ", }),

            ('7', new[] { "██████ ",
                          "██  ██ ",
                          "    ██ ",
                          "   ██  ",
                          "  ██   ",
                          "  ██   ",
                          " ████  ", }),

            ('8', new[] { " ████  ",
                          "██  ██ ",
                          "██  ██ ",
                          " ████  ",
                          "██  ██ ",
                          "██  ██ ",
                          " ████  ", }),

            ('9', new[] { " ████  ",
                          "██  ██ ",
                          "██  ██ ",
                          " █████ ",
                          "    ██ ",
                          "██  ██ ",
                          " ████  ", }),

            (':', new[] { "       ",
                          "  ██   ",
                          "  ██   ",
                          "       ",
                          "  ██   ",
                          "  ██   ",
                          "       ", }),

            ('.', new[] { "       ",
                          "       ",
                          "       ",
                          "       ",
                          "       ",
                          "  ██   ",
                          "  ██   ", })
        ),

        _ => throw new InvalidOperationException($"Undefined size: {size}")
    };

#pragma warning restore SA1500 // Braces for multi-line statements should not share line
}

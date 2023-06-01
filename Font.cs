using System.Collections.Concurrent;

namespace Time;

public class Font
{
    private Dictionary<char, string[]> _font;

    public int CharacterWidth { get; }
    public int CharacterHeight { get; }

    private Font(int characterWidth, int characterHeight)
    {
        CharacterHeight = characterHeight;
        CharacterWidth = characterWidth;
        _font = new();
    }

    public Font(int size, params (char key, string[] data)[] font)
    {
        _font = new();

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

    public string[] this[char index]
    {
        get => _font[index];
    }

    private static ConcurrentDictionary<int, Font> _fontsBySize = new();

    public static Font OfSize(int size, char digitDisplayChar = '#')
    {
        if (_fontsBySize.TryGetValue(size, out var font))
        {
            return font.WithDisplayChar(digitDisplayChar);
        }

        font = GetFontDefinition(size);
        _fontsBySize[size] = font;
        return font.WithDisplayChar(digitDisplayChar);
    }

    private Font WithDisplayChar(char digitDisplayChar)
    {
        var transformedFont = new Font(characterWidth: CharacterWidth, characterHeight: CharacterHeight);

        foreach (var key in _font.Keys)
        {
            transformedFont._font[key] = digitDisplayChar == '#'
                ? _font[key]
                : _font[key]
                    .Select(_ => _.Replace('#', digitDisplayChar))
                    .ToArray();
        }

        return transformedFont;
    }

    private static Font GetFontDefinition(int size) => size switch
    {
        1 => new(1,
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

        2 => new(2,
            ('0', new[] { " ##  ",
                          "#  # ",
                          "#  # ",
                          "#  # ",
                          " ##  " }),

            ('1', new[] { "  #  ",
                          " ##  ",
                          "  #  ",
                          "  #  ",
                          " ### " }),

            ('2', new[] { " ##  ",
                          "#  # ",
                          "  #  ",
                          " #   ",
                          "#### " }),

            ('3', new[] { " ##  ",
                          "#  # ",
                          "  #  ",
                          "#  # ",
                          " ##  " }),

            ('4', new[] { "  ## ",
                          " # # ",
                          "#  # ",
                          "#### ",
                          "   # " }),

            ('5', new[] { "#### ",
                          "#    ",
                          "###  ",
                          "   # ",
                          "###  " }),

            ('6', new[] { " ##  ",
                          "#    ",
                          "###  ",
                          "#  # ",
                          " ##  " }),

            ('7', new[] { "#### ",
                          "#  # ",
                          "  #  ",
                          "  #  ",
                          "  #  " }),

            ('8', new[] { " ##  ",
                          "#  # ",
                          " ##  ",
                          "#  # ",
                          " ##  " }),

            ('9', new[] { " ##  ",
                          "#  # ",
                          " ### ",
                          "   # ",
                          " ##  " }),

            (':', new[] { "     ",
                          "  #  ",
                          "     ",
                          "  #  ",
                          "     " }),

            ('.', new[] { "     ",
                          "     ",
                          "     ",
                          "     ",
                          "  #  " })
        ),

        3 => new(3,
            ('0', new[] { " ####  ",
                          "##  ## ",
                          "##  ## ",
                          "##  ## ",
                          "##  ## ",
                          "##  ## ",
                          " ####  " }),

            ('1', new[] { "  ##   ",
                          " ###   ",
                          "  ##   ",
                          "  ##   ",
                          "  ##   ",
                          "  ##   ",
                          " ####  " }),

            ('2', new[] { " ####  ",
                          "##  ## ",
                          "    ## ",
                          "  ###  ",
                          " ##    ",
                          "##  ## ",
                          "###### " }),

            ('3', new[] { " ####  ",
                          "##  ## ",
                          "    ## ",
                          "  ###  ",
                          "    ## ",
                          "##  ## ",
                          " ####  " }),

            ('4', new[] { "   ##  ",
                          "  ###  ",
                          " # ##  ",
                          "#  ##  ",
                          "###### ",
                          "   ##  ",
                          "  #### " }),

            ('5', new[] { "###### ",
                          "##  ## ",
                          "##     ",
                          "#####  ",
                          "    ## ",
                          "##  ## ",
                          " ####  " }),

            ('6', new[] { " ####  ",
                          "##  ## ",
                          "##     ",
                          "#####  ",
                          "##  ## ",
                          "##  ## ",
                          " ####  " }),

            ('7', new[] { "###### ",
                          "##  ## ",
                          "    ## ",
                          "   ##  ",
                          "  ##   ",
                          "  ##   ",
                          " ####  " }),

            ('8', new[] { " ####  ",
                          "##  ## ",
                          "##  ## ",
                          " ####  ",
                          "##  ## ",
                          "##  ## ",
                          " ####  " }),

            ('9', new[] { " ####  ",
                          "##  ## ",
                          "##  ## ",
                          " ##### ",
                          "    ## ",
                          "##  ## ",
                          " ####  " }),

            (':', new[] { "       ",
                          "  ##   ",
                          "  ##   ",
                          "       ",
                          "  ##   ",
                          "  ##   ",
                          "       " }),

            ('.', new[] { "       ",
                          "       ",
                          "       ",
                          "       ",
                          "       ",
                          "  ##   ",
                          "  ##   " })
        ),

        _ => throw new InvalidOperationException($"Undefined size: {size}")
    };
}

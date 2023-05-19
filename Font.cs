namespace Time;

public static class Font
{
    public static Dictionary<char, string[]> OfSize(int size)
    {
        return size switch
        {
            1 => new() {
                { '0', new[] { "0" } },
                { '1', new[] { "1" } },
                { '2', new[] { "2" } },
                { '3', new[] { "3" } },
                { '4', new[] { "4" } },
                { '5', new[] { "5" } },
                { '6', new[] { "6" } },
                { '7', new[] { "7" } },
                { '8', new[] { "8" } },
                { '9', new[] { "9" } },
                { ':', new[] { ":" } },
                { '.', new[] { "." } },
            },

            2 => new() {
                { '0', new[] { " ##  ",
                               "#  # ",
                               "#  # ",
                               "#  # ",
                               " ##  " } },

                { '1', new[] { "  #  ",
                               " ##  ",
                               "  #  ",
                               "  #  ",
                               " ### " } },

                { '2', new[] { " ##  ",
                               "#  # ",
                               "  #  ",
                               " #   ",
                               "#### " } },

                { '3', new[] { " ##  ",
                               "#  # ",
                               "  #  ",
                               "#  # ",
                               " ##  " } },

                { '4', new[] { "  ## ",
                               " # # ",
                               "#  # ",
                               "#### ",
                               "   # " } },

                { '5', new[] { "#### ",
                               "#    ",
                               "###  ",
                               "   # ",
                               "###  " } },

                { '6', new[] { " ##  ",
                               "#    ",
                               "###  ",
                               "#  # ",
                               " ##  " } },

                { '7', new[] { "#### ",
                               "#  # ",
                               "  #  ",
                               "  #  ",
                               "  #  " } },

                { '8', new[] { " ##  ",
                               "#  # ",
                               " ##  ",
                               "#  # ",
                               " ##  " } },

                { '9', new[] { " ##  ",
                               "#  # ",
                               " ### ",
                               "   # ",
                               " ##  " } },

                { ':', new[] { "     ",
                               "  #  ",
                               "     ",
                               "  #  ",
                               "     " } },

                { '.', new[] { "     ",
                               "     ",
                               "     ",
                               "     ",
                               "  #  " } },
            },

            3 => new() {
                { '0', new[] { " ####  ",
                               "##  ## ",
                               "##  ## ",
                               "##  ## ",
                               "##  ## ",
                               "##  ## ",
                               " ####  " } },

                { '1', new[] { "  ##   ",
                               " ###   ",
                               "  ##   ",
                               "  ##   ",
                               "  ##   ",
                               "  ##   ",
                               " ####  " } },

                { '2', new[] { " ####  ",
                               "##  ## ",
                               "    ## ",
                               "  ###  ",
                               " ##    ",
                               "##  ## ",
                               "###### " } },

                { '3', new[] { " ####  ",
                               "##  ## ",
                               "    ## ",
                               "  ###  ",
                               "    ## ",
                               "##  ## ",
                               " ####  " } },

                { '4', new[] { "   ##  ",
                               "  ###  ",
                               " # ##  ",
                               "#  ##  ",
                               "###### ",
                               "   ##  ",
                               "  #### " } },

                { '5', new[] { "###### ",
                               "##  ## ",
                               "##     ",
                               "#####  ",
                               "    ## ",
                               "##  ## ",
                               " ####  " } },

                { '6', new[] { " ####  ",
                               "##     ",
                               "##     ",
                               "#####  ",
                               "##  ## ",
                               "##  ## ",
                               " ####  " } },

                { '7', new[] { "###### ",
                               "##  ## ",
                               "   ##  ",
                               "  ##   ",
                               "  ##   ",
                               "  ##   ",
                               " ####  " } },

                { '8', new[] { " ####  ",
                               "##  ## ",
                               "##  ## ",
                               " ####  ",
                               "##  ## ",
                               "##  ## ",
                               " ####  " } },

                { '9', new[] { " ####  ",
                               "##  ## ",
                               "##  ## ",
                               " ##### ",
                               "    ## ",
                               "    ## ",
                               " ####  " } },

                { ':', new[] { "       ",
                               "  ##   ",
                               "  ##   ",
                               "       ",
                               "  ##   ",
                               "  ##   ",
                               "       " } },

                { '.', new[] { "       ",
                               "       ",
                               "       ",
                               "       ",
                               "       ",
                               "  ##   ",
                               "  ##   " } },
            },

            _ => throw new InvalidOperationException($"Invalid size: {size}")
        };
    }
}

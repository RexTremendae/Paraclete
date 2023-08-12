namespace Paraclete.Screens.Chess;

public static class BorderStyleDefinition
{
    public const string TopEdgeRow = nameof(TopEdgeRow);
    public const string MiddleEdgeRow = nameof(MiddleEdgeRow);
    public const string BottomEdgeRow = nameof(BottomEdgeRow);
    public const string VerticalEdgeRow = nameof(VerticalEdgeRow);

    public static readonly IReadOnlyDictionary<(Style, string), string> BorderStrings = new Dictionary<(Style, string), string>
    {
        // Double border: ╔═╗
        { (Style.Double, TopEdgeRow),                  "╔═╤╗" },
        { (Style.Double, MiddleEdgeRow),               "╟─┼╢" },
        { (Style.Double, VerticalEdgeRow),             "║ │║" },
        { (Style.Double, BottomEdgeRow),               "╚═╧╝" },

        // Single border: ┌─┐
        { (Style.Single, TopEdgeRow),                  "┌─┬┐" },
        { (Style.Single, MiddleEdgeRow),               "├─┼┤" },
        { (Style.Single, VerticalEdgeRow),             "│ ││" },
        { (Style.Single, BottomEdgeRow),               "└─┴┘" },

        // Single border, rounded corners: ╭─╮
        { (Style.SingleRoundCorners, TopEdgeRow),      "╭─┬╮" },
        { (Style.SingleRoundCorners, MiddleEdgeRow),   "├─┼┤" },
        { (Style.SingleRoundCorners, VerticalEdgeRow), "│ ││" },
        { (Style.SingleRoundCorners, BottomEdgeRow),   "╰─┴╯" },

        // Single thick border: ┏━┓
        { (Style.Thick, TopEdgeRow),                   "┏━┯┓" },
        { (Style.Thick, MiddleEdgeRow),                "┠─┼┨" },
        { (Style.Thick, VerticalEdgeRow),              "┃ │┃" },
        { (Style.Thick, BottomEdgeRow),                "┗━┷┛" },
    };

    public enum Style
    {
        Single,
        SingleRoundCorners,
        Double,
        Thick,
    }
}

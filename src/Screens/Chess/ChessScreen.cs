namespace Paraclete.Screens.Chess;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.Layouts;
using Paraclete.Painting;
using Paraclete.Menu;
using Paraclete.Menu.Chess;
using Paraclete.Ansi;

public class ChessScreen : IScreen
{
    private readonly Settings.ChessSettings _settings;

    public ChessScreen(IServiceProvider services, Settings settings)
    {
        Menu = services.GetRequiredService<ChessMenu>();
        _settings = settings.Chess;
    }

    public MenuBase Menu { get; }

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition[] { new (30) });

    public string Name => "Chess";

    public ConsoleKey Shortcut => ConsoleKey.F10;

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];

        if (paneIndex == 1)
        {
            var boardRows = GetBoardRows();
            painter.PaintRows(boardRows, pane, (2, 1));
        }

/*
        PaintPieces();

        var currentPlayerY = _margin.y + _boardHeight + 1;
        var currentPlayerColor = GetPlayerColor(_board.CurrentPlayer);

        buffer.Paint((_margin.x, currentPlayerY), "Current player: ", _settings.Colors.Text);
        buffer.Paint((_margin.x, currentPlayerY + 1), _board.CurrentPlayer, currentPlayerColor);

        buffer.Paint(
            (_margin.x + _boardWidth - 8, _margin.y + _boardHeight + 2),
            _board.IsCheck ? "CHECK!" : "      ",
            _settings.Colors.CheckIndicator);

        var messageLimit = 6;
        var messages = _messenger.GetMessages(messageLimit).ToArray();

        if (messages.Any())
        {
            var maxWidth = 60;
            var messageTop = _margin.y + _boardHeight + 4;
            buffer.Paint(
                (_margin.x, messageTop),
                @$"
                    ╭{"".PadLeft(maxWidth+2, '─')}╮
                    │{"".PadLeft(maxWidth+2)}│
                    ├{"".PadLeft(maxWidth+2, '─')}┤
                ",
                _settings.Colors.DialogBorder,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            buffer.Paint((_margin.x + 2, messageTop + 1), "Messages", _settings.Colors.Heading);
            messageTop += 3;

            foreach (var i in 0.To(messageLimit))
            {
                buffer.Paint(
                    (_margin.x, messageTop + i),
                    $"│{"".PadLeft(maxWidth+2)}│",
                    _settings.Colors.DialogBorder
                );
            }
            buffer.Paint(
                (_margin.x, messageTop + messageLimit),
                $"╰{"".PadLeft(maxWidth+2, '─')}╯",
                _settings.Colors.DialogBorder
            );

            foreach (var msg in messages)
            {
                buffer.Paint(
                    (_margin.x+2, messageTop++),
                    msg.Length > maxWidth ? msg[..maxWidth] : msg,
                    _settings.Colors.Messages);
            }
        }
*/
    };

/*
    private void PaintPieces()
    {
        var black = _settings.Colors.BlackPlayer;
        var white = _settings.Colors.WhitePlayer;

        foreach (var x in 0.To(8))
        {
            foreach (var y in 0.To(8))
            {
                var piece = _board.GetPiece(Transform((x, y)));
                if (!piece.HasValue)
                {
                    continue;
                }

                var p = piece.Value;

                buffer.Paint(
                    CalculatePaintPosition((x, y)),
                    p.Definition.Representation,
                    p.Color == PlayerColor.White ? white : black);
            }
        }
    }
*/

    private IEnumerable<AnsiString> GetBoardRows()
    {
        var topEdgeRowString = BorderStyleDefinition.BorderStrings[(_settings.BorderStyle, BorderStyleDefinition.TopEdgeRow)];
        var midEdgeRowString = BorderStyleDefinition.BorderStrings[(_settings.BorderStyle, BorderStyleDefinition.MiddleEdgeRow)];
        var vrtEdgeRowString = BorderStyleDefinition.BorderStrings[(_settings.BorderStyle, BorderStyleDefinition.VerticalEdgeRow)];
        var btmEdgeRowString = BorderStyleDefinition.BorderStrings[(_settings.BorderStyle, BorderStyleDefinition.BottomEdgeRow)];

        var topRowBuilder = new AnsiStringBuilder();
        var midRowBuilder = new AnsiStringBuilder();
        var vrtRowBuilder = new AnsiStringBuilder();
        var btmRowBuilder = new AnsiStringBuilder();
        var alphaRowBuilder = new AnsiStringBuilder();

        // vrtRowBuilder is excluded - should start with a number and not empty space
        topRowBuilder.Append("  ");
        midRowBuilder.Append("  ");
        btmRowBuilder.Append("  ");

        topRowBuilder.Append(_settings.Colors.Board).Append(topEdgeRowString[0]);
        midRowBuilder.Append(_settings.Colors.Board).Append(midEdgeRowString[0]);
        vrtRowBuilder.Append(_settings.Colors.Board).Append(vrtEdgeRowString[0]);
        btmRowBuilder.Append(_settings.Colors.Board).Append(btmEdgeRowString[0]);

        var first = true;

        1.To(8, endIsInclusive: true).Foreach(_ =>
        {
            if (first)
            {
                first = false;
            }
            else
            {
                topRowBuilder.Append(topEdgeRowString[2]);
                midRowBuilder.Append(midEdgeRowString[2]);
                vrtRowBuilder.Append(vrtEdgeRowString[2]);
                btmRowBuilder.Append(btmEdgeRowString[2]);
            }

            1.To(3, endIsInclusive: true).Foreach(_ =>
            {
                topRowBuilder.Append(topEdgeRowString[1]);
                midRowBuilder.Append(midEdgeRowString[1]);
                vrtRowBuilder.Append(vrtEdgeRowString[1]);
                btmRowBuilder.Append(btmEdgeRowString[1]);
            });
        });

        topRowBuilder.Append(topEdgeRowString[3]);
        midRowBuilder.Append(midEdgeRowString[3]);
        vrtRowBuilder.Append(vrtEdgeRowString[3]);
        btmRowBuilder.Append(btmEdgeRowString[3]);

        alphaRowBuilder
            .Append(_settings.Colors.Text)
            .Append("  ")
            .Append(' '); // spacing for left border

        1.To(8, endIsInclusive: true).Foreach(x =>
        {
            var alpha = (char)('a' + (_settings.RotateBoard ? (8 - x) : (x - 1)));
            alphaRowBuilder.Append($" {alpha}  ");
        });

        yield return alphaRowBuilder.Build();
        yield return topRowBuilder.Build();

        first = true;
        for (int x = 1; x <= 8; x++)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                yield return midRowBuilder.Build();
            }

            var num = _settings.RotateBoard ? x : 9 - x;
            yield return new AnsiStringBuilder()
                .Append(_settings.Colors.Text)
                .Append($"{num} ")
                .Append(_settings.Colors.Board)
                .Append(vrtRowBuilder.Build())
                .Append(_settings.Colors.Text)
                .Append($" {num}")
                .Build();
        }

        yield return btmRowBuilder.Build();
        yield return alphaRowBuilder.Build();
    }

/*
    private AnsiString GetPlayerColor(PlayerColor color)
    {
        return color switch {
            PlayerColor.White => _settings.Colors.WhitePlayer,
            PlayerColor.Black => _settings.Colors.BlackPlayer,
            _ => throw new InvalidDataException($"{color}")
        };
    }
*/
}

namespace Paraclete.Screens.Chess;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.Layouts;
using Paraclete.Painting;
using Paraclete.Menu;
using Paraclete.Menu.Chess;
using Paraclete.Ansi;
using Paraclete.Chess;
using System.Threading.Tasks;
using Paraclete.Chess.Scenarios;

public class ChessScreen : IScreen, IInitializer
{
    private readonly Settings.ChessSettings _settings;
    private readonly ChessBoard _board;
    private readonly PieceSelectionService _pieceSelectionService;
    private readonly ScenarioSelector _scenarioSelector;

    private ChessMenu _chessMenu;
    private SelectScenarioMenu _selectScenarioMenu;

    public ChessScreen(
        Settings settings,
        ChessBoard board,
        PieceSelectionService pieceSelectionService,
        ScenarioSelector scenarioSelector)
    {
        _settings = settings.Chess;
        _board = board;
        _pieceSelectionService = pieceSelectionService;
        _scenarioSelector = scenarioSelector;

        // These are handled in Initialize(), but the compiler does not understand that.
        _chessMenu = default!;
        _selectScenarioMenu = default!;
        Menu = default!;
    }

    public MenuBase Menu { get; private set; }

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition[] { new (30) });

    public string Name => "Chess";

    public ConsoleKey Shortcut => ConsoleKey.F10;

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];

        if (paneIndex == 1)
        {
            var boardPosition = (2, 2);
            PaintBoard(painter, pane, boardPosition);
            PaintPieces(painter, pane, boardPosition);
            PaintSelectionMarker(painter, pane, boardPosition);
        }
        else if (paneIndex == 0 && Menu == _selectScenarioMenu)
        {
            var rows = new List<AnsiString>()
            {
                AnsiSequences.ForegroundColors.White + "Select scenario",
                AnsiString.Empty,
            };

            foreach (var scenario in _scenarioSelector.Scenarios)
            {
                rows.Add(
                    (scenario == _scenarioSelector.SelectedScenario
                        ? AnsiSequences.ForegroundColors.Cyan + " > " + AnsiSequences.Reset
                        : "   ") +
                    scenario.Name);
            }

            painter.PaintRows(rows, pane, (1, 1));
        }

/*
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

    public Task Initialize(IServiceProvider services)
    {
        _chessMenu = services.GetRequiredService<ChessMenu>();
        _selectScenarioMenu = services.GetRequiredService<SelectScenarioMenu>();
        Menu = _chessMenu;
        return Task.CompletedTask;
    }

    public void StartSelectScenario()
    {
        Menu = _selectScenarioMenu;
    }

    public void FinishSelectScenario(IScenario scenario)
    {
        _board.InitializeScenario(scenario);
        Menu = _chessMenu;
    }

    public void CancelSelectScenario()
    {
        Menu = _chessMenu;
    }

    private void PaintSelectionMarker(Painter painter, Pane pane, (int x, int y) boardPosition)
    {
        var markerPosition = _pieceSelectionService.MarkerPosition;

        var (markerX, markerY) = CalculatePaintPosition(Transform(markerPosition), boardPosition);

        painter.Paint(_settings.Colors.PrimarySelection + "[", pane, (markerX - 1, markerY));
        painter.Paint(_settings.Colors.PrimarySelection + "]", pane, (markerX + 1, markerY));
    }

    private void PaintPieces(Painter painter, Pane pane, (int x, int y) boardPosition)
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

                var ansiPiece =
                    (p.color == PlayerColor.White ? white : black) +
                    p.definition.Representation.ToString() +
                    AnsiSequences.Reset;

                painter.Paint(
                    ansiPiece,
                    pane,
                    CalculatePaintPosition((x, y), boardPosition));
            }
        }
    }

    private void PaintBoard(Painter painter, Pane pane, (int, int) boardPosition)
    {
        painter.PaintRows(GetBoardRows(), pane, boardPosition);
    }

    private (int x, int y) Transform((int x, int y) position)
    {
        var x = position.x;
        var y = position.y;

        return _settings.RotateBoard
            ? (7 - x, 7 - y)
            : (x, y);
    }

    private (int x, int y) CalculatePaintPosition((int x, int y) position, (int x, int y) boardOffset)
    {
        var x = (position.x * 4) + boardOffset.x + 4;
        var y = ((7 - position.y) * 2) + boardOffset.y + 2;
        return (x, y);
    }

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

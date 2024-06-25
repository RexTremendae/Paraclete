namespace Paraclete.Screens.Chess;

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Chess;
using Paraclete.Modules.Chess;
using Paraclete.Modules.Chess.Scenarios;
using Paraclete.Painting;

public class ChessScreen(
        Settings settings,
        ChessBoard board,
        PieceSelectionService pieceSelectionService,
        PossibleMovesTracker possibleMovesTracker,
        ScenarioSelector scenarioSelector) : IScreen, IInitializer
{
    private readonly Settings.ChessSettings _settings = settings.Chess;
    private readonly ChessBoard _board = board;
    private readonly PieceSelectionService _pieceSelectionService = pieceSelectionService;
    private readonly ScenarioSelector _scenarioSelector = scenarioSelector;
    private readonly PossibleMovesTracker _possibleMovesTracker = possibleMovesTracker;

    private ChessMenu _chessMenu = default!;
    private SelectScenarioMenu _selectScenarioMenu = default!;

    public MenuBase Menu { get; private set; } = default!;

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition(30));

    public string Name => "Chess";

    public ConsoleKey Shortcut => ConsoleKey.F10;

    public static class Panes
    {
        public const int Menu = 0;
        public const int Board = 1;
    }

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];

        if (paneIndex == 1)
        {
            var boardPosition = (2, 2);
            PaintBoard(painter, pane, boardPosition);
            PaintPieces(painter, pane, boardPosition);
            PaintShadowPieces(painter, pane, boardPosition);
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

    private static (int X, int Y) CalculatePaintPosition((int X, int Y) position, (int X, int Y) boardOffset)
    {
        var x = (position.X * 4) + boardOffset.X + 4;
        var y = ((7 - position.Y) * 2) + boardOffset.Y + 2;
        return (x, y);
    }

    private void PaintSelectionMarker(Painter painter, Pane pane, (int X, int Y) boardPosition)
    {
        var markerPosition = _pieceSelectionService.MarkerPosition;

        var (markerX, markerY) = CalculatePaintPosition(Transform(markerPosition), boardPosition);

        painter.Paint(_settings.Colors.PrimarySelection + "[", pane, (markerX - 1, markerY));
        painter.Paint(_settings.Colors.PrimarySelection + "]", pane, (markerX + 1, markerY));
    }

    private void PaintPieces(Painter painter, Pane pane, (int X, int Y) boardPosition)
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
                    (p.Color == PlayerColor.White ? white : black) +
                    p.Definition.Representation.ToString() +
                    AnsiSequences.Reset;

                painter.Paint(
                    ansiPiece,
                    pane,
                    CalculatePaintPosition((x, y), boardPosition));
            }
        }
    }

    private void PaintShadowPieces(Painter painter, Pane pane, (int X, int Y) boardPosition)
    {
        var from = _pieceSelectionService.MarkerPosition;
        var piece = _board.GetPiece(from);

        if (piece?.Color != _board.CurrentPlayer)
        {
            return;
        }

        foreach (var move in _possibleMovesTracker.GetPossibleMovesFrom(from))
        {
            var pieceRepresentation = _board.GetPiece(move.From)!.Value.Definition.Representation;
            painter.Paint(
                _settings.Colors.ShadowPiece.ToString() + pieceRepresentation,
                pane,
                CalculatePaintPosition(Transform(move.To), boardPosition));
        }
    }

    private void PaintBoard(Painter painter, Pane pane, (int X, int Y) boardPosition)
    {
        painter.PaintRows(GetBoardRows(), pane, boardPosition);
    }

    private (int X, int Y) Transform((int X, int Y) position)
    {
        var x = position.X;
        var y = position.Y;

        return _settings.RotateBoard
            ? (7 - x, 7 - y)
            : (x, y);
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
}

namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Modules.GitNavigator;

public class GitRepositorySelectorPainter(Painter painter, RepositorySelector repositorySelector)
{
    private readonly Painter _painter = painter;
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly RepositorySelectorConfiguration _formatConfig = new(
        Header: AnsiSequences.ForegroundColors.White,
        Marker: AnsiSequences.ForegroundColors.Blue,
        Repositories: AnsiSequences.ForegroundColors.Yellow
    );

    public void PaintRepositoryList(Pane pane, (int x, int y) position)
    {
        var itemPadding = (pane.Size.x - position.x).ZeroFloor();

        var rows = new List<AnsiString>();
        var builder = new AnsiStringBuilder();

        rows.Add(builder
            .Clear()
            .Append(_formatConfig.Header)
            .Append("Repositories")
            .Build()
            .PadRight(itemPadding)
        );

        rows.AddRange(_repositorySelector.GetRepositories().Select(_ => builder
            .Clear()
            .Append(ResolveMarker(_))
            .Append(_formatConfig.Repositories)
            .Append(_)
            .Build()
            .PadRight(pane.Size.x - position.x)
        ));

        _painter.PaintRows(rows, pane, position, showEllipsis: true);
    }

    public AnsiString ResolveMarker(string repository)
    {
        return repository == _repositorySelector.SelectedRepository
            ? _formatConfig.Marker + "=> "
            : " - ";
    }
}

public readonly record struct RepositorySelectorConfiguration
(
    AnsiString Header,
    AnsiString Marker,
    AnsiString Repositories
);

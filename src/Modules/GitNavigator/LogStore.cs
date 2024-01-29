namespace Paraclete.Modules.GitNavigator;

using System.Text.RegularExpressions;

public class LogStore(RepositorySelector repositorySelector)
{
    private readonly RepositorySelector _repositorySelector = repositorySelector;
    private readonly List<LogLine> _logLines = [];

    public IEnumerable<LogLine> LogLines => _logLines;

    public async Task Refresh()
    {
        _logLines.Clear();

        var repository = _repositorySelector.SelectedRepository;
        if (string.IsNullOrEmpty(repository))
        {
            return;
        }

        var separator = (char)0x1F;

        var graphRegex = new Regex(@"^(?<graph>[*/|\\ ]+)(?<nongraph>[a-f0-9].*)?$");
        var prettyFormat = $"%h{separator}%ci{separator}%ae{separator}%s{separator}%d";
        var result = await ProcessRunner.ExecuteAsync(
            "git",
            args: ["log", "--oneline", "--graph", $"--pretty=format:{prettyFormat}", "-20"],
            workingDirectory: repository,
            launchExternal: false);

        var lineParts = new List<string[]>();
        var graphMaxLen = 0;

        foreach (var line in result.StdOut)
        {
            var match = graphRegex.Match(line);
            var parts = match.Groups["nongraph"].Value.Split(separator, StringSplitOptions.TrimEntries);
            var graphPart = match.Groups["graph"].Value;

            lineParts.Add([.. new[] { graphPart }.Concat(parts)]);
            graphMaxLen = int.Max(graphMaxLen, graphPart.Length);
        }

        foreach (var parts in lineParts)
        {
            var graph = parts[0].PadRight(graphMaxLen);

            if (string.IsNullOrWhiteSpace(graph))
            {
                continue;
            }

            if (parts.Length < 5)
            {
                _logLines.Add(new(
                    isGraphOnly: true,
                    graph: graph,
                    sha: string.Empty,
                    timestamp: default,
                    email: string.Empty,
                    message: string.Empty));
            }
            else
            {
                _logLines.Add(new(
                    isGraphOnly: false,
                    graph: graph,
                    sha: parts[1],
                    timestamp: DateTime.Parse(parts[2]),
                    email: parts[3],
                    message: parts[4]));

                // branch = parts[5]
            }
        }
    }
}

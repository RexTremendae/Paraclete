namespace Paraclete.Modules.GitNavigator;

using System.Text.RegularExpressions;

public class LogStore
{
    private readonly List<LogLine> _logLines = [];

    public IEnumerable<LogLine> LogLines => _logLines;

    public async Task Pull(string repository)
    {
        await ProcessRunner.ExecuteAsync(
            "git",
            args: ["pull"],
            workingDirectory: repository,
            launchExternal: false);

        await Refresh(repository);
    }

    public async Task Refresh(string repository)
    {
        _logLines.Clear();

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
                    message: string.Empty,
                    branches: []));
            }
            else
            {
                var branchesString = parts[5].Length > 2
                    ? parts[5][1..^1]
                    : parts[5];

                var branchList = new List<string>();

                if (branchesString.StartsWith("HEAD -> "))
                {
                    branchList.Add(branchesString[..4]);
                    branchesString = branchesString[8..];
                }

                if (!string.IsNullOrWhiteSpace(branchesString))
                {
                    var startIdx = 0;
                    var splitIdx = 0;

                    while (splitIdx >= 0)
                    {
                        splitIdx = branchesString.IndexOf(", ", startIdx);
                        branchList.Add(splitIdx >= 0
                            ? branchesString[startIdx..splitIdx]
                            : branchesString[startIdx..]);

                        startIdx = splitIdx + 2;
                    }
                }

                _logLines.Add(new(
                    isGraphOnly: false,
                    graph: graph,
                    sha: parts[1],
                    timestamp: DateTime.Parse(parts[2]),
                    email: parts[3],
                    message: parts[4],
                    branches: [.. branchList]));
            }
        }
    }
}

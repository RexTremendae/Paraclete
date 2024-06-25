namespace Paraclete.Modules.GitNavigator;

using Paraclete.Ansi;

public class LogStore
{
    private readonly List<LogLine> _logLines = [];
    private readonly List<AnsiString> _commitInfoLines = [];

    private bool _showOrigin;
    private string _selectedRepository = string.Empty;

    public int SelectedLogLineIndex { get; private set; }
    public IEnumerable<LogLine> LogLines => _logLines;
    public IEnumerable<AnsiString> CommitInfoLines => _commitInfoLines;

    public async Task Pull(string repository)
    {
        _selectedRepository = repository;

        await ProcessRunner.ExecuteAsync(
            "git",
            args: ["pull"],
            workingDirectory: repository,
            launchExternal: false);

        await Refresh(repository);
    }

    public async Task Refresh(string repository)
    {
        _selectedRepository = repository;

        var logLinesToFetch = 40;

        _logLines.Clear();
        SelectedLogLineIndex = 0;

        if (string.IsNullOrEmpty(repository))
        {
            return;
        }

        var separator = (char)0x1F;

        var prettyFormat = $"%h{separator}%ci{separator}%ae{separator}%s{separator}%d";
        var result = await ProcessRunner.ExecuteAsync(
            "git",
            args: ["log", "--oneline", "--graph", $"--pretty=format:{prettyFormat}", $"-{logLinesToFetch}"],
            workingDirectory: repository,
            launchExternal: false);

        var lineParts = new List<string[]>();
        var graphMaxLen = 0;

        foreach (var line in result.StdOut)
        {
            var match = GitRegex.GraphRegex().Match(line);
            var parts = match.Groups["nongraph"].Value.Split(separator, StringSplitOptions.TrimEntries);
            var graphPart = match.Groups["graph"].Value.Trim();

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

                        var branchName = splitIdx >= 0
                            ? branchesString[startIdx..splitIdx]
                            : branchesString[startIdx..];

                        if (_showOrigin || !branchName.StartsWith("origin/"))
                        {
                            branchList.Add(branchName);
                        }

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

        await RefreshCommitInfo();
    }

    public async Task SelectNextCommit()
    {
        for (; ;)
        {
            SelectedLogLineIndex++;
            if (SelectedLogLineIndex >= _logLines.Count)
            {
                SelectedLogLineIndex = _logLines.Count - 1;
                break;
            }

            if (!string.IsNullOrEmpty(_logLines[SelectedLogLineIndex].Sha))
            {
                break;
            }
        }

        await RefreshCommitInfo();
    }

    public async Task SelectPreviousCommit()
    {
        for (; ;)
        {
            SelectedLogLineIndex--;
            if (SelectedLogLineIndex < 0)
            {
                SelectedLogLineIndex = 0;
                break;
            }

            if (!string.IsNullOrEmpty(_logLines[SelectedLogLineIndex].Sha))
            {
                break;
            }
        }

        await RefreshCommitInfo();
    }

    public async Task RefreshCommitInfo()
    {
        var selectedCommit = _logLines[SelectedLogLineIndex].Sha;
        var result = await ProcessRunner.ExecuteAsync(
            "git",
            args: ["show", selectedCommit, "--name-status"],
            workingDirectory: _selectedRepository,
            launchExternal: false);

        _commitInfoLines.Clear();
        foreach (var line in result.StdOut)
        {
            var match = GitRegex.CommitFileChangeRegex().Match(line);
            if (match.Success)
            {
                var changeType = match.Groups["changetype"].Value.Trim();
                var filename = match.Groups["filename"].Value.Trim();

                var changeTypeColor = changeType[0] switch
                {
                    'A' => AnsiSequences.ForegroundColors.DarkGreen,
                    'D' => AnsiSequences.ForegroundColors.Orange,
                    'R' => AnsiSequences.ForegroundColors.DarkCyan,
                    'M' => AnsiSequences.ForegroundColors.DarkYellow,
                    _ => AnsiString.Empty,
                };
                _commitInfoLines.Add(changeTypeColor + changeType.PadRight(5) + filename.Replace("\t", "  "));
            }
            else
            {
                if (line.StartsWith("commit "))
                {
                    _commitInfoLines.Add(line[..6] + AnsiSequences.ForegroundColors.DarkCyan + line[6..]);
                }
                else
                {
                    _commitInfoLines.Add(line);
                }
            }
        }
    }

    public void SetShowOrigin(bool showOrigin)
    {
        _showOrigin = showOrigin;
    }
}

namespace Paraclete.Modules.GitNavigator;

using Paraclete.Ansi;

public class LogLine(
    bool isGraphOnly,
    string graph,
    string sha,
    string email,
    string message,
    DateTime timestamp,
    string[] branches
)
{
    public bool IsGraphOnly { get; } = isGraphOnly;
    public string Graph { get; } = graph;
    public string Sha { get; } = sha;
    public string Email { get; } = email;
    public string Message { get; } = message;
    public DateTime Timestamp { get; } = timestamp;
    public string[] Branches { get; } = branches;

    public AnsiString ToAnsiString()
    {
        var builder = new AnsiStringBuilder()
            .Append($"{Graph} ");

        if (!IsGraphOnly)
        {
            builder
                .Append(AnsiSequences.ForegroundColors.DarkCyan).Append($"{Sha} ")
                .Append(AnsiSequences.ForegroundColors.Yellow).Append($"{Message} ")
                .Append(AnsiSequences.ForegroundColors.Gray).Append($"({Email}) ")
            ;

            AppendBranches(builder);
        }

        return builder.Build();
    }

    private void AppendBranches(AnsiStringBuilder builder)
    {
        if (!Branches.Any())
        {
            return;
        }

        builder.Append(AnsiSequences.Reset).Append("[ ");

        var first = true;
        foreach (var branch in Branches)
        {
            if (!first)
            {
                builder.Append(AnsiSequences.Reset).Append(" | ");
            }

            first = false;

            builder
                .Append(branch switch {
                    "HEAD" => AnsiSequences.ForegroundColors.Cyan,
                    var _ when branch.StartsWith("origin/") => AnsiSequences.ForegroundColors.Red,
                    _  => AnsiSequences.ForegroundColors.Green,
                })
                .Append(branch);
        }

        builder.Append(AnsiSequences.Reset).Append(" ]");
    }
}

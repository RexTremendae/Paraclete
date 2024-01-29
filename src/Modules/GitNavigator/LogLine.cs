namespace Paraclete.Modules.GitNavigator;

using Paraclete.Ansi;

public class LogLine(
    bool isGraphOnly,
    string graph,
    string sha,
    string email,
    string message,
    DateTime timestamp
)
{
    public bool IsGraphOnly { get; } = isGraphOnly;
    public string Graph { get; } = graph;
    public string Sha { get; } = sha;
    public string Email { get; } = email;
    public string Message { get; } = message;
    public DateTime Timestamp { get; } = timestamp;

    public AnsiString ToAnsiString()
    {
        var builder = new AnsiStringBuilder()
            .Append($"{Graph} ");

        if (!IsGraphOnly)
        {
            builder
            .Append(AnsiSequences.ForegroundColors.DarkCyan).Append($"{Sha} ")
            .Append(AnsiSequences.ForegroundColors.Yellow).Append($"{Message} ")
            .Append(AnsiSequences.ForegroundColors.Gray).Append($"({Email})");
        }

        return builder.Build();
    }
}

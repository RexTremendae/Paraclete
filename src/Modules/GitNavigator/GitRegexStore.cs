namespace Paraclete.Modules.GitNavigator;

using System.Text.RegularExpressions;

public static partial class GitRegex
{
    [GeneratedRegex(@"^(?<graph>[*/|\\ ]+)(?<nongraph>[a-f0-9].*)?$")]
    public static partial Regex GraphRegex();

    [GeneratedRegex(@"^(?<changetype>M|A|D|R\d{3})\t(?<filename>.*$)")]
    public static partial Regex CommitFileChangeRegex();
}

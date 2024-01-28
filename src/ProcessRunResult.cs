namespace Paraclete;

public class ProcessRunResult(int exitCode, string[] stdOut, string[] stdError)
{
    public int ExitCode { get; } = exitCode;
    public string[] StdOut { get; } = stdOut;
    public string[] StdError { get; } = stdError;

    public static ProcessRunResult FromExitCode(int exitCode)
    {
        return new(exitCode, [], []);
    }
}

namespace Paraclete;

using System.Diagnostics;

public class ProcessRunner
{
    public static async Task<ProcessRunResult> ExecuteAsync(
        string filename,
        string[]? args = null,
        string workingDirectory = "",
        bool launchExternal = true)
    {
        var startinfo = new ProcessStartInfo
        {
            FileName = filename,
            CreateNoWindow = true,
            UseShellExecute = launchExternal,
            RedirectStandardError = !launchExternal,
            RedirectStandardOutput = !launchExternal,
            WorkingDirectory = workingDirectory,
        };

        foreach (var a in args ?? [])
        {
            startinfo.ArgumentList.Add(a);
        }

        var stdOut = new List<string>();
        var stdError = new List<string>();

        var process = Process.Start(startinfo)!;

        if (launchExternal)
        {
            return ProcessRunResult.FromExitCode(0);
        }

        process.OutputDataReceived += (sender, e) => stdOut.Add(e.Data ?? string.Empty);
        process.BeginOutputReadLine();

        process.ErrorDataReceived += (sender, e) => stdError.Add(e.Data ?? string.Empty);
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        return new ProcessRunResult(process.ExitCode, stdOut: [.. stdOut], stdError: [.. stdError]);
    }
}

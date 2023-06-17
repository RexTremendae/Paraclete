using System.Diagnostics;

namespace Paraclete.Menu.Shortcuts;

public abstract class StartProcessCommandBase
{
    protected Task Execute(string filename, params string[] args)
    {
        var startinfo = new ProcessStartInfo
        {
            FileName = filename,
            CreateNoWindow = true,
            UseShellExecute = true
        };
        foreach (var _ in args)
        {
            startinfo.ArgumentList.Add(_);
        }
        var p = Process.Start(startinfo);

        return Task.CompletedTask;
    }
}

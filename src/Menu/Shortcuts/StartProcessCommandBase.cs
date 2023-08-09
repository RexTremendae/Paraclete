namespace Paraclete.Menu.Shortcuts;

using System.Diagnostics;

public abstract class StartProcessCommandBase
{
    protected static Task Execute(string filename, params string[] args)
    {
        var startinfo = new ProcessStartInfo
        {
            FileName = filename,
            CreateNoWindow = true,
            UseShellExecute = true,
        };

        foreach (var a in args)
        {
            startinfo.ArgumentList.Add(a);
        }

        Process.Start(startinfo);

        return Task.CompletedTask;
    }
}

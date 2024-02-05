namespace Paraclete.Configuration;

using Paraclete.PInvoke;

public static class ConsoleConfigurator
{
    public static void Configure()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        EnsureAnsiIsEnabled();

        // Disable Ctrl+C
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
        };
    }

    private static void EnsureAnsiIsEnabled()
    {
        // Get the handle to the standard output stream
        var handle = Kernel32.GetStdHandle(Kernel32.StdOutputHandle);

        // Get the current console mode
        if (!Kernel32.GetConsoleMode(handle, out var mode))
        {
            Console.Error.WriteLine("Failed to get console mode");
            return;
        }

        // Enable the virtual terminal processing mode
        mode |= Kernel32.EnableVirtualTerminalProcessing;
        if (!Kernel32.SetConsoleMode(handle, mode))
        {
            Console.Error.WriteLine("Failed to set console mode");
            return;
        }
    }
}

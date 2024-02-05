namespace Paraclete.PInvoke;

using System.Runtime.InteropServices;

public static partial class Kernel32
{
    public const int StdOutputHandle = -11;
    public const uint EnableVirtualTerminalProcessing = 4;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
}

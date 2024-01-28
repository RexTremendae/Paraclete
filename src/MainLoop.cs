namespace Paraclete;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.Ansi;
using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;
using Paraclete.Screens;

public partial class MainLoop(IServiceProvider services)
{
    private readonly IServiceProvider _services = services;
    private readonly Painter _painter = services.GetRequiredService<Painter>();
    private readonly ScreenSelector _screenSelector = services.GetRequiredService<ScreenSelector>();
    private readonly ScreenSaver _screenSaver = services.GetRequiredService<ScreenSaver>();
    private readonly ScreenInvalidator _screenInvalidator = services.GetRequiredService<ScreenInvalidator>();
    private readonly FpsCounter _fpsCounter = services.GetRequiredService<FpsCounter>();
    private readonly DataInputter _dataInputter = services.GetRequiredService<DataInputter>();
    private readonly ShortcutsMenu _shortcutsMenu = services.GetRequiredService<ShortcutsMenu>();
    private readonly TimeSpan _repaintLoopInterval = services.GetRequiredService<Settings>().RepaintLoopInterval;
    private readonly Dictionary<ConsoleKey, SwitchScreenCommand> _switchScreenCommands = [];
    private readonly Terminator _terminator = services.GetRequiredService<Terminator>();

    private bool _quickMenuIsActive = false;
    private bool _repaintLoopIsActive;
    private bool _inputHandlingLoopIsActive;

    public async Task Run()
    {
        _screenSaver.Inactivate();
        _screenSelector.SwitchTo<HomeScreen>();
        InitializeSwitchScreenCommands();

        new Thread(async () => await RepaintLoop()).Start();
        new Thread(async () => await InputHandlingLoop()).Start();

        _inputHandlingLoopIsActive = true;
        _repaintLoopIsActive = true;

        while (_inputHandlingLoopIsActive || _repaintLoopIsActive)
        {
            await Task.Delay(100);
        }

        Console.Write(AnsiSequences.ClearScreen);
        Console.Write(AnsiSequences.EraseScrollbackBuffer);
        Console.Write(AnsiSequences.ShowCursor);
        Console.SetCursorPosition(0, 0);

        PrintExceptionInfo();
        SayBye();
    }

    private static void SayBye()
    {
        Console.WriteLine();
        Console.WriteLine(
            AnsiSequences.ForegroundColors.White + " -- " +
            AnsiSequences.ForegroundColors.Cyan + "Good bye! 👋" +
            AnsiSequences.ForegroundColors.White + " -- " +
            AnsiSequences.Reset);
        Console.WriteLine();
        Console.WriteLine();
    }

    private void PrintExceptionInfo()
    {
        new[] { ("Input handling loop", _inputHandlingException), ("Repaint loop", _repaintLoopException) }
        .Foreach<(string loopId, Exception? exception)>(_ =>
        {
            if (_.exception != null)
            {
                Console.WriteLine($"Unhandled exception occurred in " + AnsiSequences.ForegroundColors.Cyan + _.loopId + AnsiSequences.Reset + ": ");
                var title = _.exception.GetType().Name;
                var bar = AnsiSequences.ForegroundColors.White + string.Empty.PadLeft(title.Length + 2, '-') + AnsiSequences.Reset;
                Console.WriteLine(bar);
                Console.WriteLine(AnsiSequences.ForegroundColors.Red + $" {title}" + AnsiSequences.Reset);
                Console.WriteLine(bar);
                Console.WriteLine(_.exception.Message);
                Console.WriteLine();
                Console.WriteLine(AnsiSequences.ForegroundColors.Gray + (_.exception.StackTrace?.ToString() ?? string.Empty));
                Console.WriteLine();
            }
        });
    }
}

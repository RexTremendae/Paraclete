namespace Paraclete;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.Ansi;
using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;
using Paraclete.Screens;

public partial class MainLoop
{
    private readonly Painter _painter;
    private readonly ScreenSelector _screenSelector;
    private readonly ScreenSaver _screenSaver;
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly FpsCounter _fpsCounter;
    private readonly IServiceProvider _services;
    private readonly DataInputter _dataInputter;
    private readonly ShortcutsMenu _shortcutsMenu;
    private readonly TimeSpan _repaintLoopInterval;
    private readonly Dictionary<ConsoleKey, SwitchScreenCommand> _switchScreenCommands;
    private readonly Terminator _terminator;

    private bool _quickMenuIsActive = false;
    private bool _repaintLoopIsActive;
    private bool _inputHandlingLoopIsActive;

    public MainLoop(IServiceProvider services)
    {
        _services = services;
        _painter = services.GetRequiredService<Painter>();
        _screenSaver = services.GetRequiredService<ScreenSaver>();
        _screenSelector = services.GetRequiredService<ScreenSelector>();
        _screenInvalidator = services.GetRequiredService<ScreenInvalidator>();
        _fpsCounter = services.GetRequiredService<FpsCounter>();
        _dataInputter = services.GetRequiredService<DataInputter>();
        _shortcutsMenu = services.GetRequiredService<ShortcutsMenu>();
        _repaintLoopInterval = services.GetRequiredService<Settings>().RepaintLoopInterval;
        _terminator = services.GetRequiredService<Terminator>();
        _switchScreenCommands = new ();
    }

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

        Console.WriteLine(AnsiSequences.ClearScreen);

        PrintExceptionInfo();
        SayBye();
    }

    private static void SayBye()
    {
        Console.WriteLine(AnsiSequences.ShowCursor);
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

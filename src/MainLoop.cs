namespace Paraclete;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.IO;
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
    private readonly Dictionary<ConsoleKey, IScreen> _screens;
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
        _screens = new ();
    }

    public async Task Run()
    {
        _screenSaver.Inactivate();
        _screenSelector.SwitchTo<HomeScreen>();
        _screens.Clear();

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            _screens.Add(screen.Shortcut, screen);
        }

        new Thread(async () => await RepaintLoop()).Start();
        new Thread(async () => await InputHandlingLoop()).Start();

        _inputHandlingLoopIsActive = true;
        _repaintLoopIsActive = true;

        while (_inputHandlingLoopIsActive || _repaintLoopIsActive)
        {
            await Task.Delay(100);
        }
    }
}

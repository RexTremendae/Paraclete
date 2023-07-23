namespace Paraclete;

using Microsoft.Extensions.DependencyInjection;
using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;
using Paraclete.Screens;

public class MainLoop
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

    private bool _quickMenuIsActive = false;

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
        _screens = new ();
        _repaintLoopInterval = services.GetRequiredService<Settings>().RepaintLoopInterval;
    }

    public async Task Run()
    {
        _screenSaver.Inactivate();
        _screenSelector.SwitchTo<HomeScreen>();

        new Thread(async () => await RepaintLoop()).Start();

        _screens.Clear();
        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            _screens.Add(screen.Shortcut, screen);
        }

        await InputHandlingLoop();
    }

    private async Task InputHandlingLoop()
    {
        ConsoleKeyInfo key;

        for (; ; )
        {
            key = Console.ReadKey(true);

            var screenSaverWasActive = _screenSaver.IsActive;
            _screenSaver.Inactivate();

            var currentMenu = _screenSelector.SelectedScreen.Menu;

            var anyKeyMatch = false;

            var selectedCommand = ICommand.NoCommand;
            if (_dataInputter.IsActive)
            {
                await _dataInputter.Input(key);
            }
            else if ((_quickMenuIsActive ? _shortcutsMenu : currentMenu).MenuItems.TryGetValue(key.Key, out var selectedMenuCommand))
            {
                selectedCommand = selectedMenuCommand;
                anyKeyMatch = true;
            }

            var selectedScreen = IScreen.NoScreen;
            if (!_dataInputter.IsActive && _screens.TryGetValue(key.Key, out var selectedForSwitchScreen))
            {
                selectedScreen = selectedForSwitchScreen;
                anyKeyMatch = true;
            }

            if (!anyKeyMatch)
            {
                continue;
            }

            if (screenSaverWasActive)
            {
                if (!selectedCommand.IsScreenSaverResistant)
                {
                    continue;
                }
            }

            if (selectedCommand != ICommand.NoCommand)
            {
                await selectedCommand.Execute();
            }
            else if (selectedScreen != IScreen.NoScreen)
            {
                _screenSelector.SwitchTo(selectedScreen);
            }
        }
    }

    private async Task RepaintLoop()
    {
        var screenSaverIsActive = false;

        for (; ; )
        {
            _quickMenuIsActive = PInvoke.Keyboard.GetAsyncKeyState(PInvoke.Keyboard.VirtKey.TAB) != 0;

            if (_screenSaver.IsActive)
            {
                _screenSaver.PaintScreen();
                screenSaverIsActive = true;
            }
            else
            {
                if (screenSaverIsActive)
                {
                    screenSaverIsActive = false;
                    _screenInvalidator.InvalidateAll();
                }

                _painter.PaintScreen(_quickMenuIsActive);
            }

            await Task.Delay(_repaintLoopInterval);
            _fpsCounter.Update();
            _fpsCounter.Print();
        }
    }
}

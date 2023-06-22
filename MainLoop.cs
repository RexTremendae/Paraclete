using Microsoft.Extensions.DependencyInjection;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.Shortcuts;
using Paraclete.Painting;
using Paraclete.Screens;
using static System.Console;

namespace Paraclete;

public class MainLoop
{
    private readonly Painter _painter;
    private readonly ScreenSelector _screenSelector;
    private readonly ScreenSaver _screenSaver;
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly FpsCounter _fpsCounter;
    private readonly IServiceProvider _services;
    private readonly DataInputter _dataInputter;
    private readonly _ShortcutsMenu _shortcutsMenu;

    private readonly int _repaintLoopInterval;

    public MainLoop(IServiceProvider services)
    {
        _services = services;
        _painter = services.GetRequiredService<Painter>();
        _screenSaver = services.GetRequiredService<ScreenSaver>();
        _screenSelector = services.GetRequiredService<ScreenSelector>();
        _screenInvalidator = services.GetRequiredService<ScreenInvalidator>();
        _fpsCounter = services.GetRequiredService<FpsCounter>();
        _dataInputter = services.GetRequiredService<DataInputter>();
        _shortcutsMenu = services.GetRequiredService<_ShortcutsMenu>();

        _repaintLoopInterval = services.GetRequiredService<Settings>().RepaintLoopInterval;
    }

    private bool _escapeState = false;

    private async Task RepaintLoop()
    {
        var screenSaverIsActive = false;

        for(;;)
        {
            _escapeState = Keyboard.GetAsyncKeyState(Keyboard.VirtKey.ESCAPE) != 0;

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
                    _screenInvalidator.Invalidate();
                }
                _painter.PaintScreen(_escapeState);
            }

            await Task.Delay(_repaintLoopInterval);
            _fpsCounter.Update();
            _fpsCounter.Print();
        }
    }

    public async Task Run()
    {
        _screenSaver.Inactivate();
        _screenSelector.SwitchTo<HomeScreen>();

        new Thread(async () => await RepaintLoop()).Start();

        var screens = new Dictionary<ConsoleKey, IScreen>();
        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            screens.Add(screen.Shortcut, screen);
        }

        ConsoleKeyInfo key;

        for(;;)
        {
            key = ReadKey(true);

            var screenSaverWasActive = _screenSaver.IsActive;
            _screenSaver.Inactivate();

            var currentMenu = _screenSelector.SelectedScreen.Menu;

            var anyKeyMatch = false;

            ICommand selectedCommand = new NoCommand();
            if (_dataInputter.IsActive)
            {
                await _dataInputter.Input(key);
            }
            else if ((_escapeState ? _shortcutsMenu : currentMenu).MenuItems.TryGetValue(key.Key, out var selectedMenuCommand))
            {
                selectedCommand = selectedMenuCommand;
                anyKeyMatch = true;
            }

            IScreen selectedScreen = new NoScreen();
            if (!_dataInputter.IsActive && screens.TryGetValue(key.Key, out var selectedForSwitchScreen))
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

            if (selectedCommand is not NoCommand)
            {
                await selectedCommand.Execute();
            }
            else if (selectedScreen is not NoScreen)
            {
                _screenSelector.SwitchTo(selectedScreen);
            }
        }
    }

    [ExcludeFromEnumeration]
    private class NoScreen : IScreen
    {
        public MenuBase Menu => throw new NotImplementedException();
        public ILayout Layout => throw new NotImplementedException();
        public string Name => throw new NotImplementedException();
        public ConsoleKey Shortcut => throw new NotImplementedException();

        public void PaintContent(Painter visualizer)
        {
        }
    }

    [ExcludeFromEnumeration]
    private class NoCommand : ICommand
    {
        public ConsoleKey Shortcut => ConsoleKey.NoName;

        public string Description => string.Empty;

        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}

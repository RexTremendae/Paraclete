using Paraclete.Layouts;
using Paraclete.Menu;
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

    private readonly int _repaintLoopInterval;

    public MainLoop(Painter painter, ScreenSelector screenSelector, ScreenSaver screenSaver, ScreenInvalidator screenInvalidator, FpsCounter fpsCounter, Settings settings, IServiceProvider services)
    {
        _painter = painter;
        _screenSaver = screenSaver;
        _screenSelector = screenSelector;
        _screenInvalidator = screenInvalidator;
        _fpsCounter = fpsCounter;
        _repaintLoopInterval = settings.RepaintLoopInterval;
        _services = services;
    }

    private async Task RepaintLoop()
    {
        var screenSaverIsActive = false;

        for(;;)
        {
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
                _painter.PaintScreen();
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

        ConsoleKey key;

        var screens = new Dictionary<ConsoleKey, IScreen>();
        var functionKey = ConsoleKey.F1;
        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            screens.Add(functionKey, screen);
            functionKey++;
        }

        for(;;)
        {
            key = ReadKey(true).Key;

            var screenSaverWasActive = _screenSaver.IsActive;
            _screenSaver.Inactivate();

            var currentMenu = _screenSelector.SelectedScreen.Menu.MenuItems;

            var anyKeyMatch = false;
            if (currentMenu.TryGetValue(key, out var selectedCommand))
            {
                anyKeyMatch = true;
            }
            else
            {
                selectedCommand = new NoCommand();
            }

            if (screens.TryGetValue(key, out var selectedScreen))
            {
                anyKeyMatch = true;
            }
            else
            {
                selectedScreen = new NoScreen();
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

            if (selectedScreen is not NoScreen)
            {
                _screenSelector.SwitchTo(selectedScreen);
            }
            else if (selectedCommand is not NoCommand)
            {
                await selectedCommand.Execute();
            }
        }
    }

    [ExcludeFromEnumeration]
    private class NoScreen : IScreen
    {
        public MenuBase Menu => throw new NotImplementedException();
        public ILayout Layout => throw new NotImplementedException();

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

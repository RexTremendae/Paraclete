using Time.Screens;
using static System.Console;

namespace Time;

public class MainLoop
{
    private readonly Painter _painter;
    private readonly ScreenSelector _screenSelector;
    private readonly ScreenSaver _screenSaver;
    private readonly FrameInvalidator _frameInvalidator;
    private readonly FpsCounter _fpsCounter;

    private readonly int _repaintLoopInterval;

    public MainLoop(Painter painter, ScreenSelector screenSelector, ScreenSaver screenSaver, FrameInvalidator frameInvalidator, FpsCounter fpsCounter, Settings settings)
    {
        _painter = painter;
        _screenSaver = screenSaver;
        _screenSelector = screenSelector;
        _frameInvalidator = frameInvalidator;
        _fpsCounter = fpsCounter;
        _repaintLoopInterval = settings.RepaintLoopInterval;
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
                    _frameInvalidator.Invalidate();
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

        for(;;)
        {
            key = ReadKey(true).Key;

            var screenSaverWasActive = _screenSaver.IsActive;
            _screenSaver.Inactivate();

            var currentMenu = _screenSelector.SelectedScreen.Menu.MenuItems;

            if (!currentMenu.TryGetValue(key, out var selectedCommand))
            {
                continue;
            }

            if (screenSaverWasActive && !selectedCommand.IsScreenSaverResistant)
            {
                continue;
            }

            await selectedCommand.Execute();
        }
    }
}

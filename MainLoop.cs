using Time.Screens;
using static System.Console;

namespace Time;

public class MainLoop
{
    private readonly Painter _visualizer;
    private readonly ScreenSelector _screenSelector;
    private readonly ScreenSaver _screenSaver;
    private readonly FrameInvalidator _frameInvalidator;

    public MainLoop(Painter visualizer, ScreenSelector screenSelector, ScreenSaver screenSaver, FrameInvalidator frameInvalidator)
    {
        _visualizer = visualizer;
        _screenSaver = screenSaver;
        _screenSelector = screenSelector;
        _frameInvalidator = frameInvalidator;
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
                _visualizer.PaintScreen();
            }

            await Task.Delay(30);
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

            _screenSaver.Inactivate();

            var currentMenu = _screenSelector.SelectedScreen.Menu.MenuItems;

            if (!currentMenu.TryGetValue(key, out var selectedCommand))
            {
                continue;
            }

            await selectedCommand.Execute();
        }
    }
}

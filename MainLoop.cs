using Time.Menu;
using static System.Console;

namespace Time;

public class MainLoop
{
    private readonly Visualizer _visualizer;
    private readonly MainMenu _mainMenu;
    private readonly ScreenSaver _screenSaver;
    private readonly FrameInvalidator _frameInvalidator;

    public MainLoop(Visualizer visualizer, MainMenu mainMenu, ScreenSaver screenSaver, FrameInvalidator frameInvalidator)
    {
        _visualizer = visualizer;
        _mainMenu = mainMenu;
        _screenSaver = screenSaver;
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
        var commands = _mainMenu.MenuItems.ToDictionary(key => key.Shortcut, value => value);

        new Thread(async () => await RepaintLoop()).Start();

        ConsoleKey key;

        for(;;)
        {
            key = ReadKey(true).Key;

            if (key == ConsoleKey.Escape) return;

            _screenSaver.Inactivate();

            if (!commands.TryGetValue(key, out var selectedCommand))
            {
                continue;
            }

            await selectedCommand.Execute();
        }
    }
}

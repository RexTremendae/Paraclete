using Time.Menu;
using static System.Console;

namespace Time;

public class MainLoop
{
    private readonly Visualizer _visualizer;
    private readonly MainMenu _mainMenu;

    public MainLoop(Visualizer visualizer, MainMenu mainMenu)
    {
        _visualizer = visualizer;
        _mainMenu = mainMenu;
    }

    private async Task RepaintLoop()
    {
        for(;;)
        {
            _visualizer.PaintScreen();
            await Task.Delay(30);
        }
    }

    public async Task Run()
    {
        var commands = _mainMenu.MenuItems.ToDictionary(key => key.Shortcut, value => value);

        new Thread(async () => await RepaintLoop()).Start();

        ConsoleKey key;

        for(;;)
        {
            key = ReadKey(true).Key;

            if (key == ConsoleKey.Escape) return;

            if (!commands.TryGetValue(key, out var selectedCommand))
            {
                continue;
            }

            await selectedCommand.Execute();
        }
    }
}

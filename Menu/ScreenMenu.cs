using Paraclete.Screens;

namespace Paraclete.Menu;

public static class ScreenMenu
{
    public static IEnumerable<(ConsoleKey shortcut, IScreen screen)> Get(IServiceProvider services)
    {
        var functionKey = ConsoleKey.F1;
        var commands = new List<Type>();
        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(services))
        {
            yield return (functionKey, screen);
            functionKey++;
        }
    }
}

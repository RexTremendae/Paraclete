namespace Paraclete;

using Paraclete.Menu;
using Paraclete.Screens;

public partial class MainLoop
{
    private Exception? _inputHandlingException;

    private void InitializeSwitchScreenCommands()
    {
        _switchScreenCommands.Clear();

        foreach (var screen in TypeUtility.EnumerateImplementatingInstancesOf<IScreen>(_services))
        {
            _switchScreenCommands.Add(screen.Shortcut, new(screen.Shortcut, screen, _screenSelector));
        }
    }

    private async Task InputHandlingLoop()
    {
        ConsoleKeyInfo key;

        while (true)
        {
            if (_terminator.TerminationRequested)
            {
                break;
            }

            try
            {
                key = Console.ReadKey(true);

                var screenSaverWasActive = _screenSaver.IsActive;
                _screenSaver.Inactivate();

                var selectedCommand = await HandleInput(key);

                if (screenSaverWasActive && !selectedCommand.IsScreenSaverResistant)
                {
                    continue;
                }

                if (selectedCommand != ICommand.NoCommand)
                {
                    await selectedCommand.Execute();
                }
            }
            catch (Exception ex)
            {
                _inputHandlingException = ex;
                _terminator.RequestTermination();
            }
        }

        _inputHandlingLoopIsActive = false;
    }

    private async Task<ICommand> HandleInput(ConsoleKeyInfo key)
    {
        var currentMenu = _screenSelector.SelectedScreen.Menu;

        if (_dataInputter.IsActive)
        {
            await _dataInputter.Input(key);
            return ICommand.NoCommand;
        }

        var menuItems = (_quickMenuIsActive ? _shortcutsMenu : currentMenu).MenuItems;
        var menuCommand = menuItems.TryGetValue(key.Key, out var selectedMenuCommand)
            ? selectedMenuCommand
            : ICommand.NoCommand;

        return _switchScreenCommands.TryGetValue(key.Key, out var selectedSwitchCommand)
            ? selectedSwitchCommand
            : menuCommand;
    }
}

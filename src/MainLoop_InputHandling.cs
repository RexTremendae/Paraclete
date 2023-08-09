namespace Paraclete;

using Menu;
using Screens;

public partial class MainLoop
{
    private async Task InputHandlingLoop()
    {
        ConsoleKeyInfo key;

        for (; ; )
        {
            if (_terminator.TerminationRequested)
            {
                break;
            }

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

            if (screenSaverWasActive && !selectedCommand.IsScreenSaverResistant)
            {
                continue;
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

        _inputHandlingLoopIsActive = false;
    }
}

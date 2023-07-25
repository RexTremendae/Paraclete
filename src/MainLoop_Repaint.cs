namespace Paraclete;

public partial class MainLoop
{
    private async Task RepaintLoop()
    {
        var screenSaverIsActive = false;

        for (; ; )
        {
            if (_terminator.TerminationRequested)
            {
                break;
            }

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

        _repaintLoopIsActive = false;
    }
}

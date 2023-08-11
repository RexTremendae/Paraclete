namespace Paraclete;

public partial class MainLoop
{
    private async Task RepaintLoop()
    {
        var screenSaverWasActive = false;

        while (true)
        {
            if (_terminator.TerminationRequested)
            {
                break;
            }

            _quickMenuIsActive = PInvoke.Keyboard.GetAsyncKeyState(PInvoke.Keyboard.VirtKey.TAB) != 0;

            if (_screenSaver.IsActive)
            {
                _screenSaver.PaintScreen();
                screenSaverWasActive = true;
            }
            else
            {
                using var x = _screenInvalidator.BeginPaint();
                {
                    if (screenSaverWasActive)
                    {
                        screenSaverWasActive = false;
                        _screenInvalidator.InvalidateAll();
                    }

                    _painter.PaintScreen(_quickMenuIsActive);
                }
            }

            await Task.Delay(_repaintLoopInterval);
            _fpsCounter.Update();
            _fpsCounter.Print();
        }

        _repaintLoopIsActive = false;
    }
}

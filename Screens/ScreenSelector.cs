using Microsoft.Extensions.DependencyInjection;

namespace Paraclete.Screens;

public class ScreenSelector
{
    private IScreen? _selectedScreen;
    public IScreen SelectedScreen => _selectedScreen ?? throw new InvalidOperationException("No screen selected.");

    public void SwitchTo(IScreen screen)
    {
        if (screen == _selectedScreen)
        {
            return;
        }

        _selectedScreen = screen;
        _frameInvalidator.Invalidate();
    }

    public void SwitchTo<T>()
        where T : IScreen
    {
        var screenToSelect = _serviceProvider.GetRequiredService<T>();
        SwitchTo(screenToSelect);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly FrameInvalidator _frameInvalidator;

    public ScreenSelector(IServiceProvider serviceProvider, FrameInvalidator frameInvalidator)
    {
        _serviceProvider = serviceProvider;
        _frameInvalidator = frameInvalidator;
    }
}

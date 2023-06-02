using Microsoft.Extensions.DependencyInjection;

namespace Time.Screens;

public class ScreenSelector
{
    private ScreenBase? _selectedScreen;
    public ScreenBase SelectedScreen => _selectedScreen ?? throw new InvalidOperationException("No screen selected.");

    public void SwitchTo<T>()
        where T : ScreenBase
    {
        _selectedScreen = _serviceProvider.GetRequiredService<T>();
        _frameInvalidator.Invalidate();
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly FrameInvalidator _frameInvalidator;

    public ScreenSelector(IServiceProvider serviceProvider, FrameInvalidator frameInvalidator)
    {
        _serviceProvider = serviceProvider;
        _frameInvalidator = frameInvalidator;
    }
}

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
        _selectedScreen.OnAfterSwitch();
        _screenInvalidator.Invalidate();
    }

    public void SwitchTo<T>()
        where T : IScreen
    {
        var screenToSelect = _serviceProvider.GetRequiredService<T>();
        SwitchTo(screenToSelect);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ScreenInvalidator _screenInvalidator;

    public ScreenSelector(IServiceProvider serviceProvider, ScreenInvalidator screenInvalidator)
    {
        _serviceProvider = serviceProvider;
        _screenInvalidator = screenInvalidator;
    }
}

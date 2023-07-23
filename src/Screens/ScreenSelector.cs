namespace Paraclete.Screens;

using Microsoft.Extensions.DependencyInjection;

public class ScreenSelector
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ScreenInvalidator _screenInvalidator;

    private IScreen? _selectedScreen;

    public ScreenSelector(IServiceProvider serviceProvider, ScreenInvalidator screenInvalidator)
    {
        _serviceProvider = serviceProvider;
        _screenInvalidator = screenInvalidator;
    }

    public IScreen SelectedScreen => _selectedScreen ?? throw new InvalidOperationException("No screen selected.");

    public void SwitchTo(IScreen screen)
    {
        if (screen == _selectedScreen)
        {
            return;
        }

        _selectedScreen = screen;
        _selectedScreen.OnAfterSwitch();

        _screenInvalidator.InvalidateAll();
    }

    public void SwitchTo<T>()
        where T : IScreen
    {
        var screenToSelect = _serviceProvider.GetRequiredService<T>();
        SwitchTo(screenToSelect);
    }
}

namespace Paraclete.Screens;

using Microsoft.Extensions.DependencyInjection;

public class ScreenSelector(IServiceProvider serviceProvider, ScreenInvalidator screenInvalidator)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

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

        _screenInvalidator.InvalidateAll();
    }

    public void SwitchTo<T>()
        where T : IScreen
    {
        var screenToSelect = _serviceProvider.GetRequiredService<T>();
        SwitchTo(screenToSelect);
    }
}

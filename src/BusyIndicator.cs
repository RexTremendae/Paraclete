namespace Paraclete;

using Paraclete.Screens;

public class BusyIndicator(ScreenInvalidator screenInvalidator)
{
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    private readonly Dictionary<Type, Dictionary<int, string>> _busyTexts = [];

    public (bool IsBusy, string BusyText) IsPaneBusy<T>(int paneIndex)
        where T : IScreen
    {
        return IsPaneBusy(typeof(T), paneIndex);
    }

    public (bool IsBusy, string BusyText) IsPaneBusy(Type screenType, int paneIndex)
    {
        return GetPaneBusyTextsForType(screenType).TryGetValue(paneIndex, out var text)
            ? (true, text)
            : (false, string.Empty);
    }

    public void IndicatePaneIsReady<T>(int paneIndex)
        where T : IScreen
    {
        GetPaneBusyTextsForType<T>().Remove(paneIndex);
    }

    public IDisposable IndicatePaneIsBusy<T>(int paneIndex, string text = "Working...")
        where T : IScreen
    {
        GetPaneBusyTextsForType<T>().Add(paneIndex, text);
        _screenInvalidator.InvalidatePane(paneIndex);

        return new BusyIndicatorDisposable<T>(this, _screenInvalidator, paneIndex);
    }

    private Dictionary<int, string> GetPaneBusyTextsForType<T>()
        where T : IScreen
    {
        return GetPaneBusyTextsForType(typeof(T));
    }

    private Dictionary<int, string> GetPaneBusyTextsForType(Type paneType)
    {
        if (!_busyTexts.TryGetValue(paneType, out var busyPanes))
        {
            busyPanes = [];
            _busyTexts.Add(paneType, busyPanes);
        }

        return busyPanes;
    }

    private class BusyIndicatorDisposable<T>(BusyIndicator busyIndicator, ScreenInvalidator screenInvalidator, int paneIndex)
        : IDisposable
        where T : IScreen
    {
        private readonly BusyIndicator _busyIndicator = busyIndicator;
        private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
        private readonly int _paneIndex = paneIndex;

        public void Dispose()
        {
            _busyIndicator.IndicatePaneIsReady<T>(_paneIndex);
            _screenInvalidator.InvalidatePane(_paneIndex);
        }
    }
}

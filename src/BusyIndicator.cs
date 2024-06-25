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

    public void IndicatePanesAreReady<T>(params int[] paneIndices)
        where T : IScreen
    {
        foreach (var idx in paneIndices)
        {
            GetPaneBusyTextsForType<T>().Remove(idx);
        }
    }

    public IDisposable IndicatePanesAreBusy<T>(string text, params int[] paneIndices)
        where T : IScreen
    {
        var texts = GetPaneBusyTextsForType<T>();
        foreach (var pIdx in paneIndices)
        {
            texts.Add(pIdx, text);
            _screenInvalidator.InvalidatePanes(paneIndices);
        }

        return new BusyIndicatorDisposable<T>(this, _screenInvalidator, paneIndices);
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

    private class BusyIndicatorDisposable<T>(BusyIndicator busyIndicator, ScreenInvalidator screenInvalidator, int[] paneIndices)
        : IDisposable
        where T : IScreen
    {
        private readonly BusyIndicator _busyIndicator = busyIndicator;
        private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;
        private readonly int[] _paneIndices = paneIndices;

        public void Dispose()
        {
            _busyIndicator.IndicatePanesAreReady<T>(_paneIndices);
            _screenInvalidator.InvalidatePanes(_paneIndices);
        }
    }
}

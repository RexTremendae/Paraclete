namespace Paraclete.Screens.Showroom;

public class ExhibitionSelector(IServiceProvider services, ScreenInvalidator screenInvalidator)
{
    private readonly List<IExhibition> _exhibitions = new(TypeUtility.EnumerateImplementatingInstancesOf<IExhibition>(services));
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public int ExhibitionCount => _exhibitions.Count;
    public int SelectedExhibitionIndex { get; private set; }
    public IExhibition SelectedExhibition => _exhibitions[SelectedExhibitionIndex];

    public void SelectNext()
    {
        SelectedExhibitionIndex++;

        if (SelectedExhibitionIndex >= _exhibitions.Count)
        {
            SelectedExhibitionIndex = 0;
        }

        _screenInvalidator.InvalidateAll();
    }

    public void SelectPrevious()
    {
        SelectedExhibitionIndex--;

        if (SelectedExhibitionIndex < 0)
        {
            SelectedExhibitionIndex = _exhibitions.Count - 1;
        }

        _screenInvalidator.InvalidateAll();
    }
}

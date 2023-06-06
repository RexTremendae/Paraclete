namespace Paraclete.Screens;

public class ExhibitionSelector
{
    private readonly List<IExhibition> Exhibitions;
    private readonly ScreenInvalidator _screenInvalidator;

    public int ExhibitionCount => Exhibitions.Count;
    public int SelectedExhibitionIndex { get; private set; }
    public IExhibition SelectedExhibition => Exhibitions[SelectedExhibitionIndex];

    public void SelectNext()
    {
        SelectedExhibitionIndex++;
        if (SelectedExhibitionIndex >= Exhibitions.Count) SelectedExhibitionIndex = 0;
        _screenInvalidator.Invalidate();
    }

    public void SelectPrevious()
    {
        SelectedExhibitionIndex--;
        if (SelectedExhibitionIndex < 0) SelectedExhibitionIndex = Exhibitions.Count-1;
        _screenInvalidator.Invalidate();
    }

    public ExhibitionSelector(IServiceProvider services, ScreenInvalidator screenInvalidator)
    {
        Exhibitions = new(TypeUtility.EnumerateImplementatingInstancesOf<IExhibition>(services));
        _screenInvalidator = screenInvalidator;
    }
}

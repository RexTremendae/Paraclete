namespace Time.Screens;

public class ExhibitionSelector
{
    private readonly List<IExhibition> Exhibitions;
    private readonly FrameInvalidator _frameInvalidator;

    public int ExhibitionCount => Exhibitions.Count;
    public int SelectedExhibitionIndex { get; private set; }
    public IExhibition SelectedExhibition => Exhibitions[SelectedExhibitionIndex];

    public void SelectNext()
    {
        SelectedExhibitionIndex++;
        if (SelectedExhibitionIndex >= Exhibitions.Count) SelectedExhibitionIndex = 0;
        _frameInvalidator.Invalidate();
    }

    public void SelectPrevious()
    {
        SelectedExhibitionIndex--;
        if (SelectedExhibitionIndex < 0) SelectedExhibitionIndex = Exhibitions.Count-1;
        _frameInvalidator.Invalidate();
    }

    public ExhibitionSelector(IServiceProvider services, FrameInvalidator frameInvalidator)
    {
        Exhibitions = new(TypeUtility.EnumerateImplementatingInstancesOf<IExhibition>(services));
        _frameInvalidator = frameInvalidator;
    }
}

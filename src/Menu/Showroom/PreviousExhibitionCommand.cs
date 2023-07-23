namespace Paraclete.Menu.Showroom;

using Paraclete.Screens.Showroom;

public class PreviousExhibitionCommand : ICommand
{
    private readonly ExhibitionSelector _exhibitionSelector;

    public PreviousExhibitionCommand(ExhibitionSelector exhibitionSelector)
    {
        _exhibitionSelector = exhibitionSelector;
    }

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;
    public string Description => "Prev exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectPrevious();
        return Task.CompletedTask;
    }
}
namespace Paraclete.Menu.Showroom;

using Paraclete.Screens.Showroom;

public class PreviousExhibitionCommand(ExhibitionSelector exhibitionSelector)
    : ICommand
{
    private readonly ExhibitionSelector _exhibitionSelector = exhibitionSelector;

    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;
    public string Description => "Prev exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectPrevious();
        return Task.CompletedTask;
    }
}
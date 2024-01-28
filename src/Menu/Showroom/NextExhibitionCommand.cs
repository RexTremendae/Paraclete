namespace Paraclete.Menu.Showroom;

using Paraclete.Screens.Showroom;

public class NextExhibitionCommand(ExhibitionSelector exhibitionSelector)
    : ICommand
{
    private readonly ExhibitionSelector _exhibitionSelector = exhibitionSelector;

    public ConsoleKey Shortcut => ConsoleKey.RightArrow;
    public string Description => "Next exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectNext();
        return Task.CompletedTask;
    }
}
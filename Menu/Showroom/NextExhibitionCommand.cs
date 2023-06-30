namespace Paraclete.Menu.Showroom;

using Paraclete.Screens;

public class NextExhibitionCommand : ICommand
{
    private readonly ExhibitionSelector _exhibitionSelector;

    public NextExhibitionCommand(ExhibitionSelector exhibitionSelector)
    {
        _exhibitionSelector = exhibitionSelector;
    }

    public ConsoleKey Shortcut => ConsoleKey.RightArrow;
    public string Description => "Next exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectNext();
        return Task.CompletedTask;
    }
}
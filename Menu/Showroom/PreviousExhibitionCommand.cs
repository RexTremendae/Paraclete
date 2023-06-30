namespace Paraclete.Menu.Showroom;

using Paraclete.Screens;

public class PreviousExhibitionCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.LeftArrow;

    public string Description => "Prev exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectPrevious();
        return Task.CompletedTask;
    }

    private readonly ExhibitionSelector _exhibitionSelector;

    public PreviousExhibitionCommand(ExhibitionSelector exhibitionSelector)
    {
        _exhibitionSelector = exhibitionSelector;
    }
}
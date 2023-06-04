using Time.Screens;

namespace Time.Menu.Showroom;

public class NextExhibitionCommand : ICommand
{
    public ConsoleKey Shortcut => ConsoleKey.RightArrow;

    public string Description => "Next exhibition";

    public Task Execute()
    {
        _exhibitionSelector.SelectNext();
        return Task.CompletedTask;
    }

    private readonly ExhibitionSelector _exhibitionSelector;

    public NextExhibitionCommand(ExhibitionSelector exhibitionSelector)
    {
        _exhibitionSelector = exhibitionSelector;
    }
}
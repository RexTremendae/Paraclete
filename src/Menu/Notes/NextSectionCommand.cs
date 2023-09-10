namespace Paraclete.Menu.Notes;

public class NextSectionCommand : ICommand
{
    private readonly Notebook _notebook;
    private readonly ScreenInvalidator _screenInvalidator;

    public NextSectionCommand(Notebook notebook, ScreenInvalidator screenInvalidator)
    {
        _notebook = notebook;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Next section";

    public Task Execute()
    {
        _notebook.SelectNextSection();
        _screenInvalidator.InvalidateAll();

        return Task.CompletedTask;
    }
}

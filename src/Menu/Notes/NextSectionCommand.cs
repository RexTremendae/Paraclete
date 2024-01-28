namespace Paraclete.Menu.Notes;

public class NextSectionCommand(Notebook notebook, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly Notebook _notebook = notebook;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.DownArrow;
    public string Description => "Next section";

    public Task Execute()
    {
        _notebook.SelectNextSection();
        _screenInvalidator.InvalidateAll();

        return Task.CompletedTask;
    }
}

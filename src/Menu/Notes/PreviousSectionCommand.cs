namespace Paraclete.Menu.Notes;

public class PreviousSectionCommand(Notebook notebook, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly Notebook _notebook = notebook;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Prev section";

    public Task Execute()
    {
        _notebook.SelectPreviousSection();
        _screenInvalidator.InvalidateAll();

        return Task.CompletedTask;
    }
}

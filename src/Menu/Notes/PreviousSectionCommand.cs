namespace Paraclete.Menu.Notes;

public class PreviousSectionCommand : ICommand
{
    private readonly Notebook _notebook;
    private readonly ScreenInvalidator _screenInvalidator;

    public PreviousSectionCommand(Notebook notebook, ScreenInvalidator screenInvalidator)
    {
        _notebook = notebook;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.UpArrow;
    public string Description => "Prev section";

    public Task Execute()
    {
        _notebook.SelectPreviousSection();
        _screenInvalidator.InvalidateAll();

        return Task.CompletedTask;
    }
}

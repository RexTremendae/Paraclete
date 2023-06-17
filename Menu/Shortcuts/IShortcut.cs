namespace Paraclete.Menu.Shortcuts;

public interface IShortcut : ICommand
{
    string LongDescription { get; }
}

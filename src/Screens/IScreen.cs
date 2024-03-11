namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Painting;

public interface IScreen
{
    public static readonly IScreen NoScreen = new NoScreenImplementation();

    MenuBase Menu { get; }
    ILayout Layout { get; }
    string Name { get; }
    ConsoleKey Shortcut { get; }
    bool ShowCurrentTime => true;
    bool ShowTitle => true;
    int[] AutoRefreshingPaneIndices => [];

    public virtual void OnAfterSwitch()
    {
    }

    public Action GetPaintPaneAction(Painter painter, int paneIndex);

    [ExcludeFromEnumeration]
    private sealed class NoScreenImplementation : IScreen
    {
        public MenuBase Menu => throw new NotSupportedException();
        public ILayout Layout => throw new NotSupportedException();
        public string Name => throw new NotSupportedException();
        public ConsoleKey Shortcut => throw new NotSupportedException();
        public bool ShowCurrentTime => throw new NotSupportedException();

        public Action GetPaintPaneAction(Painter painter, int paneIndex) =>
        () =>
        {
        };
    }
}

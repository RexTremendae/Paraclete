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

    public virtual void OnAfterSwitch()
    {
    }

    void PaintContent(Painter painter);

    [ExcludeFromEnumeration]
    private class NoScreenImplementation : IScreen
    {
        public MenuBase Menu => throw new NotSupportedException();
        public ILayout Layout => throw new NotSupportedException();
        public string Name => throw new NotSupportedException();
        public ConsoleKey Shortcut => throw new NotSupportedException();

        public void PaintContent(Painter visualizer)
        {
        }
    }
}

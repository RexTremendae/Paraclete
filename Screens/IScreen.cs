using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Painting;

namespace Paraclete.Screens;

public interface IScreen
{
    MenuBase Menu { get; }
    ILayout Layout { get; }
    string Name { get; }
    int Ordinal { get; }
    ConsoleKey Shortcut { get; }

    public virtual void OnAfterSwitch() { }
    void PaintContent(Painter painter);
}

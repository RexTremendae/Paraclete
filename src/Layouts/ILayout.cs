namespace Paraclete.Layouts;

using Paraclete.Painting;

public interface ILayout
{
    Pane[] Panes { get; }
    void Recalculate(int windowWidth, int windowHeight);
    void Paint(Painter painter);
}

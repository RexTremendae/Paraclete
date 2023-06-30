namespace Paraclete.Layouts;

using Paraclete.Painting;

public interface ILayout
{
    void Paint(Painter painter, int windowWidth, int windowHeight);
}

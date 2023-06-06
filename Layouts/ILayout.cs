using Paraclete.Painting;

namespace Paraclete.Layouts;

public interface ILayout
{
    void Paint(Painter painter, int windowWidth, int windowHeight);
}

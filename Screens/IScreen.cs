using Paraclete.Menu;
using Paraclete.Painting;

namespace Paraclete.Screens;

public interface IScreen
{
    MenuBase Menu { get; }
    string Name => "";

    void PaintContent(Painter visualizer);
    void PaintFrame(Painter visualizer, int windowWidth, int windowHeight);
}

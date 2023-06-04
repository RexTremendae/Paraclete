using Time.Menu;
using Time.Painting;

namespace Time.Screens;

public interface IScreen
{
    MenuBase Menu { get; }
    string Name => "";

    void PaintContent(Painter visualizer);
    void PaintFrame(Painter visualizer, int windowWidth, int windowHeight);
}

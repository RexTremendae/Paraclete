using Time.Menu;

namespace Time.Screens;

public interface IScreen
{
    MenuBase Menu { get; }

    void PaintContent(Painter visualizer);
    void PaintFrame(Painter visualizer, int windowWidth, int windowHeight);
}

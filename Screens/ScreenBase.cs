using Time.Menu;

namespace Time.Screens;

public abstract class ScreenBase
{
    public abstract MenuBase Menu { get; }

    public abstract void PaintContent(Painter visualizer);
    public abstract void PaintFrame(Painter visualizer, int windowWidth, int windowHeight);
}

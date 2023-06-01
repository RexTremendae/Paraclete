using Time.Menu;

namespace Time.Screens;

public abstract class ScreenBase
{
    public abstract IMenu Menu { get; }

    public abstract void PaintContent(Visualizer visualizer);
    public abstract void PaintFrame(Visualizer visualizer, int windowWidth, int windowHeight);
}

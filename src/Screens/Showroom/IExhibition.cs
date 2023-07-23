namespace Paraclete.Screens.Showroom;

using Paraclete.Layouts;
using Paraclete.Painting;

public interface IExhibition
{
    ILayout Layout { get; }
    void Paint(Painter painter, (int x, int y) position, int paneIndex);
}

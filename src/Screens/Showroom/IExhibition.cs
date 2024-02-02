namespace Paraclete.Screens.Showroom;

using Paraclete.Layouts;
using Paraclete.Painting;

public interface IExhibition
{
    ILayout Layout { get; }
    void Paint(Painter painter, (int X, int Y) position, int paneIndex);
}

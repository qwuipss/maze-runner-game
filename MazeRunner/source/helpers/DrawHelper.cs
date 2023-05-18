using MazeRunner.Cameras;
using Microsoft.Xna.Framework;
using System.Drawing;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Helpers;

public static class DrawHelper
{
    public static bool IsInViewBox(Vector2 position, RectangleXna sourceRectangle, RectangleF viewBox)
    {
        var drawBox = new RectangleF(position.X, position.Y, sourceRectangle.Width, sourceRectangle.Height);

        return viewBox.IntersectsWith(drawBox);
    }

    public static RectangleF GetViewBox(ICamera camera)
    {
        var position = camera.Position;

        var halfViewWidth = camera.ViewWidth / 2;
        var halfViewHeight = camera.ViewHeight / 2;

        return new RectangleF(
            position.X - halfViewWidth,
            position.Y - halfViewHeight,
            camera.ViewWidth,
            camera.ViewHeight);
    }
}

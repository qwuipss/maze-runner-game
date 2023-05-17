using Microsoft.Xna.Framework;
using System;
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

    public static RectangleF GetViewBox(Vector2 offset, int viewWidth, int viewHeight)
    {
        var halfViewWidth = viewWidth / 2;
        var halfViewHeight = viewHeight / 2;

        return new RectangleF(
            offset.X - halfViewWidth,
            offset.Y - halfViewHeight,
            viewWidth,
            viewHeight);
    }
}

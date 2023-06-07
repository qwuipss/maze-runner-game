using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using System.Drawing;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Helpers;

public static class HitBoxHelper
{
    public static RectangleF GetHitBox(Vector2 position, float offsetX, float offsetY, float width, float height)
    {
        var newPosition = new Vector2(position.X + offsetX, position.Y + offsetY);

        return GetHitBox(newPosition, width, height);
    }

    public static RectangleF GetHitBox(Vector2 position, float width, float height)
    {
        return new RectangleF(position.X, position.Y, width, height);
    }

    public static RectangleXna GetArea<T>(Cell center, int widthRadius, int heightRadius, T[,] bounds)
    {
        var areaWidth = bounds.GetLength(1) - 1;
        var areaHeight = bounds.GetLength(0) - 1;

        var x = center.X - widthRadius;
        x = x < 0 ? 0 : x;

        var width = widthRadius * 2;
        width = x + width > areaWidth ? areaWidth - x : width;

        var y = center.Y - heightRadius;
        y = y < 0 ? 0 : y;

        var height = heightRadius * 2;
        height = y + height > areaHeight ? areaHeight - y : height;

        return new RectangleXna(x, y, width, height);
    }
}

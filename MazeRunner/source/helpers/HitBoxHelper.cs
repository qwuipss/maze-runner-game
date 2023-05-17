using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.Helpers;

public static class HitBoxHelper
{
    public static RectangleF GetHitBox(Vector2 position, int offsetX, int offsetY, int width, int height)
    {
        var newPosition = new Vector2(position.X + offsetX, position.Y + offsetY);

        return GetHitBox(newPosition, width, height);
    }

    public static RectangleF GetHitBox(Vector2 position, int width, int height)
    {
        return new RectangleF(position.X, position.Y, width, height);
    }
}

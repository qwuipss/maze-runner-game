using MazeRunner.Extensions;
using Microsoft.Xna.Framework;

namespace MazeRunner.Helpers;

public static class HitBoxHelper
{
    public static FloatRectangle GetHitBox(Vector2 position, int offsetX, int offsetY, int width, int height)
    {
        return new FloatRectangle(position.X + offsetX, position.Y + offsetY, width, height);
    }

    public static FloatRectangle GetHitBox(Vector2 position, int width, int height)
    {
        return new FloatRectangle(position.X, position.Y, width, height);
    }
}

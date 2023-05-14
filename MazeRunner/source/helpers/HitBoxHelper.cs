using Microsoft.Xna.Framework;

namespace MazeRunner.Helpers;

public static class HitBoxHelper
{
    public static Rectangle GetHitBox(Vector2 position, int offsetX, int offsetY, int width, int height)
    {
        return new Rectangle((int)position.X + offsetX, (int)position.Y + offsetY, width, height);
    }

    public static Rectangle GetHitBox(Vector2 position, int width, int height)
    {
        return new Rectangle((int)position.X, (int)position.Y, width, height);
    }
}

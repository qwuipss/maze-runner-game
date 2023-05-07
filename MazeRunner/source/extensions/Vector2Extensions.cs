using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Abs(this Vector2 vector)
    {
        return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
    }
}

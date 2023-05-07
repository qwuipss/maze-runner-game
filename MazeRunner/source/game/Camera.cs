#region Usings
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace MazeRunner;

public class Camera
{
    public Matrix TransformMatrix { get; private set; }

    public void Follow(Sprite sprite, Vector2 position, int screenWidth, int screenHeight)
    {
        var cameraPosition = Matrix.CreateTranslation(
            -position.X - (sprite.FrameWidth / 2),
            -position.Y - (sprite.FrameHeight / 2),
            0);

        var offset = Matrix.CreateTranslation(
            screenWidth / 2,
            screenHeight / 2,
            0);

        TransformMatrix = cameraPosition * offset;
    }
}

using MazeRunner.Cameras;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner;

public class Camera : ICamera
{
    private readonly Vector3 _origin;

    private readonly Matrix _scale;

    private readonly Matrix _bordersOffset;

    private Matrix _transformMatrix;

    public Camera(Viewport viewPort, float scaleCoeff = 1)
    {
        _scale = Matrix.CreateScale(new Vector3(scaleCoeff, scaleCoeff, 0));

        _origin = new Vector3(viewPort.Width / 2, viewPort.Height / 2, 0);
        _bordersOffset = Matrix.CreateTranslation(_origin);
    }

    public void Follow(Sprite sprite, Vector2 position)
    {
        var cameraPosition = Matrix.CreateTranslation(
            -position.X - (sprite.FrameWidth / 2),
            -position.Y - (sprite.FrameHeight / 2),
            0);

        _transformMatrix = cameraPosition * _scale * _bordersOffset;
    }

    public Matrix GetTransformMatrix()
    {
        return _transformMatrix;
    }
}
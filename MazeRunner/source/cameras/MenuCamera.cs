using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class MenuCamera : ICamera
{
    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private Matrix _transformMatrix;

    private Vector2 _viewPosition;

    public Vector2 ViewPosition
    {
        get
        {
            return _viewPosition;
        }
    }

    public int ViewWidth
    {
        get
        {
            return _viewWidth;
        }
    }

    public int ViewHeight
    {
        get
        {
            return _viewHeight;
        }
    }

    public Matrix TransformMatrix
    {
        get
        {
            return _transformMatrix;
        }
    }

    public MenuCamera(GraphicsDevice graphicsDevice)
    {
        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        var bordersOffset = Matrix.CreateTranslation(0, 0, 0);

        var position = Matrix.CreateTranslation(0, 0, 0);

        _viewPosition = Vector2.Zero;

        _transformMatrix = position * bordersOffset;
    }
}

using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class MenuCamera : MazeRunnerGameComponent, ICamera
{
    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private Matrix _transformMatrix;

    private Vector2 _viewPosition;

    private readonly Texture2D _effect;

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

        _viewPosition = new Vector2(_viewWidth / 2, _viewHeight / 2);

        var shadowTreshold = _viewHeight / 2.1f;

        _effect = EffectsHelper.CreateGradientCircleEffect(_viewWidth, _viewHeight, shadowTreshold, graphicsDevice);

        _transformMatrix = position * bordersOffset;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.Draw(_effect, Vector2.Zero, new Rectangle(0, 0, _viewWidth, _viewHeight), 0);
    }
}

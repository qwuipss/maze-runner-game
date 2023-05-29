using MazeRunner.Components;
using MazeRunner.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class StaticCamera : MazeRunnerGameComponent, ICamera
{
    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private Matrix _transformMatrix;

    private Vector2 _viewPosition;

    public Vector2 ViewPosition => _viewPosition;

    public int ViewWidth => _viewWidth;

    public int ViewHeight => _viewHeight;

    public Matrix TransformMatrix => _transformMatrix;

    public Texture2D Effect { get; set; }

    public StaticCamera(GraphicsDevice graphicsDevice)
    {
        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        var bordersOffset = Matrix.CreateTranslation(0, 0, 0);

        var position = Matrix.CreateTranslation(0, 0, 0);

        _viewPosition = new Vector2(_viewWidth / 2, _viewHeight / 2);

        _transformMatrix = position * bordersOffset;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
        if (Effect is not null)
        {
            Drawer.Draw(Effect, Vector2.Zero, new Rectangle(0, 0, _viewWidth, _viewHeight), 0);
        }
    }
}

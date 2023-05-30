using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class HeroCamera : MazeRunnerGameComponent, ICamera
{
    private const float DrawingPriority = .1f;

    private readonly Hero _hero;

    private readonly Matrix _scale;

    private readonly Matrix _bordersOffset;

    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private Matrix _transformMatrix;

    private Vector2 _viewPosition;

    public Vector2 ViewPosition => _viewPosition;

    public int ViewWidth => _viewWidth;

    public int ViewHeight => _viewHeight;

    public Matrix TransformMatrix => _transformMatrix;

    public Texture2D Effect { get; set; }

    public HeroCamera(GraphicsDevice graphicsDevice, Hero hero)
    {
        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        _bordersOffset = Matrix.CreateTranslation(_viewWidth / 2, _viewHeight / 2, 0);

        var scaleFactor = 7;

        _scale = Matrix.CreateScale(scaleFactor, scaleFactor, 0);

        _hero = hero;
    }

    public override void Draw(GameTime gameTime)
    {
        var viewBox = DrawHelper.GetViewBox(this);
        var position = new Vector2(viewBox.X, viewBox.Y);

        Drawer.Draw(Effect, position, new Rectangle(0, 0, _viewWidth, _viewHeight), DrawingPriority);
    }

    public override void Update(GameTime gameTime)
    {
        FollowHero();
    }

    private void FollowHero()
    {
        var heroPosition = _hero.Position;

        var halfFrameSize = _hero.FrameSize / 2;

        var cameraPosition = Matrix.CreateTranslation(
            -heroPosition.X - halfFrameSize,
            -heroPosition.Y - halfFrameSize,
            0);

        _viewPosition = new Vector2(heroPosition.X + halfFrameSize, heroPosition.Y + halfFrameSize);
        _transformMatrix = cameraPosition * _scale * _bordersOffset;
    }
}
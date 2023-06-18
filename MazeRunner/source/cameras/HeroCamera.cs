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

    public float EffectTransparency { get; set; }

    public HeroCamera(Hero hero, int viewWidth, int viewHeight)
    {
        EffectTransparency = 1;

        _viewWidth = viewWidth;
        _viewHeight = viewHeight;

        _bordersOffset = Matrix.CreateTranslation(_viewWidth / 2, _viewHeight / 2, 0);

        var scaleDivider = 275f;
        var scaleFactor = viewWidth / scaleDivider;

        _scale = Matrix.CreateScale(scaleFactor, scaleFactor, 0);

        _hero = hero;

        Position = _hero.Position;
    }

    public override void Draw(GameTime gameTime)
    {
        var viewBox = DrawHelper.GetViewBox(this);
        var position = new Vector2(viewBox.X, viewBox.Y);

        Drawer.Draw(Effect, position, new Rectangle(0, 0, _viewWidth, _viewHeight), DrawingPriority, transparency: EffectTransparency);
    }

    public override void Update(GameTime gameTime)
    {
        FollowHero();

        Position = _hero.Position;
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
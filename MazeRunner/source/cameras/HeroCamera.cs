using MazeRunner.Components;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class HeroCamera : MazeRunnerGameComponent, ICamera
{
#pragma warning disable CS0067
    public override event GameComponentProvider NeedDisposeNotify;
#pragma warning disable

    private readonly Matrix _scale;

    private readonly Matrix _bordersOffset;

    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private Matrix _transformMatrix;

    private Vector2 _position;

    public Vector2 Position
    {
        get
        {
            return _position;
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

    public HeroCamera(Viewport viewPort, float scaleFactor = 1)
    {
        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        var origin = new Vector2(_viewWidth / 2, _viewHeight / 2);

        _bordersOffset = Matrix.CreateTranslation(new Vector3(origin, 0));
        _scale = Matrix.CreateScale(new Vector3(scaleFactor, scaleFactor, 0));
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        var hero = game.HeroInfo.Sprite;

        Follow(hero, game.HeroInfo.Position);
    }

    private void Follow(Sprite sprite, Vector2 spritePosition)
    {
        var halfFrameSize = sprite.FrameSize / 2;

        var cameraPosition = Matrix.CreateTranslation(
            -spritePosition.X - halfFrameSize,
            -spritePosition.Y - halfFrameSize,
            0);

        _position = new Vector2(spritePosition.X + halfFrameSize, spritePosition.Y + halfFrameSize);
        _transformMatrix = cameraPosition * _scale * _bordersOffset;
    }
}
using MazeRunner.Components;
using MazeRunner.Helpers;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class HeroCamera : MazeRunnerGameComponent, ICamera
{
    private const float DrawingPriority = .1f;

    private readonly SpriteInfo _heroInfo;

    private readonly Texture2D _effect;

    private readonly Matrix _scale;

    private readonly Matrix _bordersOffset;

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

    public HeroCamera(GraphicsDevice graphicsDevice, float shadowTreshold, SpriteInfo heroInfo, float scaleFactor = 1)
    {
        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        _bordersOffset = Matrix.CreateTranslation(_viewWidth / 2, _viewHeight / 2, 0);

        _scale = Matrix.CreateScale(scaleFactor, scaleFactor, 0);

        _effect = EffectsHelper.CreateGradientCircleEffect(_viewWidth, _viewHeight, shadowTreshold, graphicsDevice);

        _heroInfo = heroInfo;
    }

    public override void Draw(GameTime gameTime)
    {
        var viewBox = DrawHelper.GetViewBox(this);
        var position = new Vector2(viewBox.X, viewBox.Y);

        //Drawer.Draw(_effect, position, new Rectangle(0, 0, _viewWidth, _viewHeight), DrawingPriority);
    }

    public override void Update(GameTime gameTime)
    {
        Follow(_heroInfo.Sprite, _heroInfo.Position);
    }

    private void Follow(Sprite sprite, Vector2 spritePosition)
    {
        var halfFrameSize = sprite.FrameSize / 2;

        var cameraPosition = Matrix.CreateTranslation(
            -spritePosition.X - halfFrameSize,
            -spritePosition.Y - halfFrameSize,
            0);

        _viewPosition = new Vector2(spritePosition.X + halfFrameSize, spritePosition.Y + halfFrameSize);
        _transformMatrix = cameraPosition * _scale * _bordersOffset;
    }
}
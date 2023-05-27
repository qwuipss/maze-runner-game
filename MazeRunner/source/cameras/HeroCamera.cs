using MazeRunner.Components;
using MazeRunner.Extensions;
using MazeRunner.GameBase.States;
using MazeRunner.Helpers;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class HeroCamera : MazeRunnerGameComponent, ICamera
{
#pragma warning disable CS0067
    public override event GameComponentProvider NeedDisposeNotify;
#pragma warning disable

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

    public HeroCamera(Viewport viewPort, float shadowTreshold, GraphicsDevice graphicsDevice, SpriteInfo heroInfo, float scaleFactor = 1)
    {
        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        var origin = new Vector2(_viewWidth / 2, _viewHeight / 2);

        _bordersOffset = Matrix.CreateTranslation(new Vector3(origin, 0));
        _scale = Matrix.CreateScale(new Vector3(scaleFactor, scaleFactor, 0));

        _effect = CreateEffect(shadowTreshold, graphicsDevice);

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

    private Texture2D CreateEffect(float shadowTreshold, GraphicsDevice graphicsDevice)
    {
        var effectData = new Color[_viewHeight, _viewWidth];
        var centerPixel = new Vector2(_viewWidth / 2, _viewHeight / 2);

        for (int y = 0; y < effectData.GetLength(0); y++)
        {
            for (int x = 0; x < effectData.GetLength(1); x++)
            {
                var currentPixel = new Vector2(x, y);
                var distance = Vector2.Distance(centerPixel, currentPixel);

                if (distance >= shadowTreshold)
                {
                    effectData[y, x] = Color.Black;
                }
                else
                {
                    var transparentCoeff = distance / shadowTreshold;
                    var transparency = (byte)((float)byte.MaxValue * transparentCoeff);

                    effectData[y, x] = Color.Black;
                    effectData[y, x].A = transparency;
                }
            }
        }

        var effectTexture = new Texture2D(graphicsDevice, _viewWidth, _viewHeight);

        effectTexture.SetData(effectData.ToLinear());

        return effectTexture;
    }
}
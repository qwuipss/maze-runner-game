using MazeRunner.Cameras;
using MazeRunner.Content;
using MazeRunner.GameBase.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public class HeroHealthWriter : TextWriter
{
    private static readonly Texture2D _heartTexture;

    private readonly float _scaleFactor;

    private readonly StaticCamera _staticCamera;

    private readonly GameRunningState _runningState;

    private readonly Hero _hero;

    private int _count;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => $"x{_count}";

    static HeroHealthWriter()
    {
        _heartTexture = Textures.Marks.Heart;
    }

    public HeroHealthWriter(Hero hero, GameRunningState runningState, float scaleDivider, GraphicsDevice graphicsDevice)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        _scaleFactor = graphicsDevice.Viewport.Width / scaleDivider;

        _runningState = runningState;

        _staticCamera = new StaticCamera(graphicsDevice);

        var textOffset = 1.25f;

        Position = new Vector2(_heartTexture.Width * _scaleFactor * textOffset, 0);
    }

    public override void Draw(GameTime gameTime)
    {
        GameRunningState.SwitchCamera(_staticCamera);

        Drawer.Draw(_heartTexture, Vector2.Zero, new Rectangle(0, 0, _heartTexture.Width, _heartTexture.Height), DrawingPriority, scale: _scaleFactor);

        Drawer.DrawString(this);

        _runningState.ContinueDraw();
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.Health)
        {
            _count = _hero.Health;
        }
    }
}

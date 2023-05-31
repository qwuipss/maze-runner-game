using MazeRunner.Cameras;
using MazeRunner.Content;
using MazeRunner.GameBase.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public class HeroChalkUsesWriter : TextWriter
{
    private static readonly Texture2D _chalkTexture;

    private readonly float _scaleFactor;

    private readonly StaticCamera _staticCamera;

    private readonly GameRunningState _runningState;

    private readonly Hero _hero;

    private readonly Vector2 _chalkTextureDrawingPosition;

    private int _count;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => $"x{_count}";

    static HeroChalkUsesWriter()
    {
        _chalkTexture = Textures.Gui.StateShowers.Chalk;
    }

    public HeroChalkUsesWriter(Hero hero, GameRunningState runningState, HeroHealthWriter healthWriter, float scaleDivider, GraphicsDevice graphicsDevice)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        _count = _hero.ChalkUses;

        _scaleFactor = graphicsDevice.Viewport.Width / scaleDivider;

        _runningState = runningState;

        _staticCamera = new StaticCamera(graphicsDevice);

        var textOffset = 1.25f;

        var topOffset = 1.5f;

        var stringSize = Font.MeasureString($"x{_count}") * _scaleFactor;

        _chalkTextureDrawingPosition = new Vector2(
            0, 
            healthWriter.HeartTextureDrawingPosition.Y + HeroHealthWriter.HeartTexture.Height * healthWriter.ScaleFactor * topOffset);

        Position = new Vector2(
            _chalkTexture.Width * _scaleFactor * textOffset,
            _chalkTextureDrawingPosition.Y + _chalkTexture.Height * _scaleFactor - stringSize.Y * .86f);
    }

    public override void Draw(GameTime gameTime)
    {
        GameRunningState.SwitchCamera(_staticCamera);

        Drawer.Draw(_chalkTexture, _chalkTextureDrawingPosition, new Rectangle(0, 0, _chalkTexture.Width, _chalkTexture.Height), DrawingPriority, scale: _scaleFactor);

        Drawer.DrawString(this);

        _runningState.ContinueDraw();
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.ChalkUses)
        {
            _count = _hero.ChalkUses;
        }
    }
}

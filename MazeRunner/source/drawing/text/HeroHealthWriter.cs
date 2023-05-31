using MazeRunner.Cameras;
using MazeRunner.Content;
using MazeRunner.GameBase.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing.Writers;

public class HeroHealthWriter : TextWriter
{
    public static readonly Texture2D HeartTexture;

    private readonly float _scaleFactor;

    private readonly StaticCamera _staticCamera;

    private readonly Hero _hero;

    private int _count;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => $"x{_count}";

    public Vector2 HeartTextureDrawingPosition { get; init; }

    static HeroHealthWriter()
    {
        HeartTexture = Textures.Gui.StateShowers.Heart;
    }

    public HeroHealthWriter(Hero hero, float scaleDivider, GraphicsDevice graphicsDevice)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        _count = _hero.Health;

        _scaleFactor = graphicsDevice.Viewport.Width / scaleDivider;

        _staticCamera = new StaticCamera(graphicsDevice);

        var textOffset = 1.25f;

        var stringSize = Font.MeasureString($"x{_count}") * _scaleFactor;

        Position = new Vector2(HeartTexture.Width * _scaleFactor * textOffset, HeartTexture.Height * _scaleFactor - stringSize.Y * .86f);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(_staticCamera);

        Drawer.Draw(HeartTexture, HeartTextureDrawingPosition, new Rectangle(0, 0, HeartTexture.Width, HeartTexture.Height), DrawingPriority, scale: _scaleFactor);

        Drawer.DrawString(this);

        Drawer.EndDraw();
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.Health)
        {
            _count = _hero.Health;
        }
    }
}

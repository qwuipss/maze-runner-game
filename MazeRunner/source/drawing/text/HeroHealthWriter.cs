using MazeRunner.Content;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace MazeRunner.Drawing;

public class HeroHealthWriter : TextWriter
{
    private static readonly Texture2D _heartTexture;

    private readonly Hero _hero;

    private readonly int _viewWidth;

    private readonly int _viewHeight;

    private int _count;

    static HeroHealthWriter()
    {
        _heartTexture = Textures.Marks.Heart;
    }

    public HeroHealthWriter(Hero hero, GraphicsDevice graphicsDevice)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;
    }

    public override float ScaleFactor => 1;

    public override string Text => $"x{_count}";

    public override void Draw(GameTime gameTime)
    {
        // Drawer.DrawString(this);
        Drawer.Draw(_heartTexture, Position, new Rectangle(0, 0, _heartTexture.Width, _heartTexture.Height), .05f);
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.Health)
        {
            _count = _hero.Health;
        }

        GetHeartDrawingPosition();
    }

    private void GetHeartDrawingPosition()
    {
        var heroPosition = _hero.Position;
        var halfFrameSize = _hero.FrameSize / 2;

        var normalizedPosition = new Vector2(heroPosition.X + halfFrameSize, heroPosition.Y + halfFrameSize);

        Position = new Vector2(normalizedPosition.X - _viewWidth / 7, normalizedPosition.Y - _viewHeight / 7);
    }
}

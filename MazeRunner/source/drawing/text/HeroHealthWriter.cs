using MazeRunner.Content;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Drawing.Writers;

public class HeroHealthWriter : TextWriter
{
    public static readonly Texture2D HeartTexture;

    private readonly float _scaleFactor;

    private readonly Hero _hero;

    private int _count;

#pragma warning disable CS0067 // The event 'HeroHealthWriter.WriterDiedNotify' is never used
    public override event Action WriterDiedNotify;
#pragma warning restore CS0067 // The event 'HeroHealthWriter.WriterDiedNotify' is never used

    public override float ScaleFactor => _scaleFactor;

    public override string Text => $"x{_count}";

    public Vector2 HeartTextureDrawingPosition { get; init; }

    static HeroHealthWriter()
    {
        HeartTexture = Textures.Gui.StateShowers.Heart;
    }

    public HeroHealthWriter(Hero hero, float scaleDivider, int viewWidth)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        _count = _hero.Health;

        _scaleFactor = viewWidth / scaleDivider;

        var textOffset = 1.25f;

        var stringSize = Font.MeasureString($"x{_count}") * _scaleFactor;

        var textDowningCoeff = .86f;

        Position = new Vector2(HeartTexture.Width * _scaleFactor * textOffset, HeartTexture.Height * _scaleFactor - stringSize.Y * textDowningCoeff);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.Draw(
            HeartTexture,
            HeartTextureDrawingPosition,
            new Rectangle(0, 0, HeartTexture.Width, HeartTexture.Height),
            DrawingPriority,
            scale: _scaleFactor);

        Drawer.DrawString(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.Health)
        {
            _count = _hero.Health;
        }
    }
}

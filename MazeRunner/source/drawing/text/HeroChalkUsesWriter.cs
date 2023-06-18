using MazeRunner.Content;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Drawing.Writers;

public class HeroChalkUsesWriter : TextWriter
{
    public static readonly Texture2D ChalkTexture;

    private readonly float _scaleFactor;

    private readonly Hero _hero;

    private int _count;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => $"x{_count}";

    public Vector2 ChalkTextureDrawingPosition { get; init; }

    static HeroChalkUsesWriter()
    {
        ChalkTexture = Textures.Gui.StateShowers.Chalk;
    }

    public HeroChalkUsesWriter(Hero hero, HeroHealthWriter healthWriter, float scaleDivider, int viewWidth)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;

        _count = _hero.ChalkUses;

        _scaleFactor = viewWidth / scaleDivider;

        var textOffset = 1.25f;

        var topOffset = 1.5f;

        var stringSize = Font.MeasureString($"x{_count}") * _scaleFactor;

        ChalkTextureDrawingPosition = new Vector2(
            0,
            healthWriter.HeartTextureDrawingPosition.Y + HeroHealthWriter.HeartTexture.Height * healthWriter.ScaleFactor * topOffset);

        var textDowningCoeff = .86f;

        Position = new Vector2(
            ChalkTexture.Width * _scaleFactor * textOffset,
            ChalkTextureDrawingPosition.Y + ChalkTexture.Height * _scaleFactor - stringSize.Y * textDowningCoeff);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.Draw(
            ChalkTexture,
            ChalkTextureDrawingPosition,
            new Rectangle(0, 0, ChalkTexture.Width, ChalkTexture.Height),
            DrawingPriority,
            scale: _scaleFactor);

        Drawer.DrawString(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.ChalkUses)
        {
            _count = _hero.ChalkUses;
        }
    }
}

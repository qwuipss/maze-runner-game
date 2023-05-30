using MazeRunner.Content;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.Drawing;

public class HeroHealthWriter : TextWriter
{
    private readonly Hero _hero;

    private int _count;

    public HeroHealthWriter(Hero hero)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = hero;
    }

    public override float ScaleFactor => 1;

    public override string Text => $"x{_count}";

    public override void Draw(GameTime gameTime)
    {
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

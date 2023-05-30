using MazeRunner.Content;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Drawing;

public class HeroHealthWriter : TextWriter
{
    private readonly Hero _hero;

    private int _count;

    public HeroHealthWriter(SpriteInfo heroInfo)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _hero = (Hero)heroInfo.Sprite;
    }

    public override float ScaleFactor => 1;

    public override string Text => $"x{_count}";

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawString(this, Position);
    }

    public override void Update(GameTime gameTime)
    {
        if (_count != _hero.Health)
        {
            _count = _hero.Health;
        }
    }
}

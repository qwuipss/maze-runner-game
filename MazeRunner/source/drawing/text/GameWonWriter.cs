using MazeRunner.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Drawing.Writers;

public class GameWonWriter : TextWriter
{
    private readonly float _scaleFactor;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => "Game won";

    public GameWonWriter(int viewWidth, int viewHeight)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        var scaleDivider = 250;

        _scaleFactor = viewWidth / scaleDivider;

        var stringSize = Font.MeasureString(Text) * _scaleFactor;

        Position = new Vector2((viewWidth - stringSize.X) / 2, (viewHeight - stringSize.Y) / 2 * .45f);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawString(this);
    }

    public override void Update(GameTime gameTime)
    {
    }
}

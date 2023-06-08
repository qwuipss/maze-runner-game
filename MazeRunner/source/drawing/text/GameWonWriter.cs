using MazeRunner.Content;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Drawing.Writers;

public class GameWonWriter : TextWriter
{
    private readonly float _scaleFactor;

#pragma warning disable CS0067 // The event 'GameWonWriter.WriterDiedNotify' is never used
    public override event Action WriterDiedNotify;
#pragma warning restore CS0067 // The event 'GameWonWriter.WriterDiedNotify' is never used

    public override float ScaleFactor => _scaleFactor;

    public override string Text => "Game won";

    public GameWonWriter(int viewWidth, int viewHeight)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        var scaleDivider = 250;

        _scaleFactor = viewWidth / scaleDivider;

        var stringSize = Font.MeasureString(Text) * _scaleFactor;

        var topOffset = .45f;

        Position = new Vector2((viewWidth - stringSize.X) / 2, (viewHeight - stringSize.Y) / 2 * topOffset);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawString(this);
    }

    public override void Update(GameTime gameTime)
    {
    }
}

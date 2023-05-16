using MazeRunner.Components;
using MazeRunner.Drawing;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Wrappers;

public class TextWriterInfo : MazeRunnerGameComponent
{
    public override event GameComponentProvider NeedDisposeNotify;

    public Vector2 Position { get; set; }

    private TextWriter TextWriter { get; init; }

    public TextWriterInfo(TextWriter textWriter)
    {
        TextWriter = textWriter;
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        TextWriter.Update(game, gameTime);

        if (TextWriter.IsDead)
        {
            NeedDisposeNotify.Invoke(this);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        TextWriter.Draw(gameTime, Position);
    }
}
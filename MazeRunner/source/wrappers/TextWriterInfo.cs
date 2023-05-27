using MazeRunner.Components;
using MazeRunner.Drawing;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class TextWriterInfo : MazeRunnerGameComponent
{
    public TextWriter TextWriter { get; init; }

    public Vector2 Position { get; set; }

    public TextWriterInfo(TextWriter textWriter)
    {
        TextWriter = textWriter;
    }

    public override void Update(GameTime gameTime)
    {
        TextWriter.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        TextWriter.Draw(gameTime, Position);
    }
}
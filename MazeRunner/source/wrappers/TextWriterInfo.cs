using MazeRunner.Components;
using MazeRunner.Drawing;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class TextWriterInfo : MazeRunnerGameComponent
{
    private TextWriter TextWriter { get; init; }

    private Vector2 Position { get; set; }

    public TextWriterInfo(TextWriter textWriter, Vector2 position)
    {
        TextWriter = textWriter;
        Position = position;
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        TextWriter.Update(game, gameTime);

        Position = game.TextWritersPositions[TextWriter];
    }

    public override void Draw(GameTime gameTime)
    {
        TextWriter.Draw(gameTime, Position);
    }
}
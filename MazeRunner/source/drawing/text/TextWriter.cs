using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public abstract class TextWriter
{
    public virtual float DrawingPriority
    {
        get
        {
            return 0;
        }
    }

    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public virtual SpriteFont Font { get; init; }

    public virtual Color Color { get; init; }

    public abstract void Update(MazeRunnerGame game, GameTime gameTime);

    public abstract void Draw(GameTime gameTime, Vector2 position);
}
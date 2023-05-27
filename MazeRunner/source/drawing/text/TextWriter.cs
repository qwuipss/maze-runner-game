using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public abstract class TextWriter
{
    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public SpriteFont Font { get; init; }

    public Color Color { get; init; }

    public virtual float DrawingPriority
    {
        get
        {
            return 0.05f;
        }
    }

    public bool IsDead { get; protected set; }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, Vector2 position);
}
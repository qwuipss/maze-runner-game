using MazeRunner.GameBase.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public abstract class TextWriter
{
    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public virtual float DrawingPriority
    {
        get
        {
            return 0;
        }
    }

    public virtual SpriteFont Font { get; init; }

    public virtual Color Color { get; init; }

    public virtual bool IsDead { get; protected set; }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, Vector2 position);
}
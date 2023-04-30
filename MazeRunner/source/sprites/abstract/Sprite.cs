#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract ISpriteState State { get; set; }

    public abstract Vector2 Position { get; set; }

    public abstract Vector2 Speed { get; }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int Width
    {
        get
        {
            return State.Width;
        }
    }

    public virtual int Height
    {
        get
        {
            return State.Height;
        }
    }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public virtual Point GetCurrentAnimationPoint(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs >= State.AnimationDelayMs)
        {
            State = ProcessState();
            ElapsedGameTimeMs -= State.AnimationDelayMs;
        }

        return State.CurrentAnimationPoint;
    }

    public virtual ISpriteState ProcessState()
    {
        return State.ProcessState();
    }
}
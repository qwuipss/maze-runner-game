#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract ISpriteState State { get; set; }

    public abstract Vector2 Speed { get; }

    public abstract Vector2 Position { get; set; }

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
            return State.FrameWidth;
        }
    }

    public virtual int Height
    {
        get
        {
            return State.FrameHeight;
        }
    }

    public virtual SpriteEffects FrameEffect { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public virtual Point GetCurrentAnimationPoint(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs >= State.FrameAnimationDelayMs)
        {
            State = ProcessState();
            ElapsedGameTimeMs -= State.FrameAnimationDelayMs;
        }

        return State.CurrentAnimationFramePoint;
    }

    public virtual ISpriteState ProcessState()
    {
        return State.ProcessState();
    }
}
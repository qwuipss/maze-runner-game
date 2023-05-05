#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract Vector2 Speed { get; }

    protected abstract ISpriteState State { get; set; }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int FrameWidth
    {
        get
        {
            return State.FrameWidth;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return State.FrameHeight;
        }
    }

    public virtual SpriteEffects FrameEffect { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public virtual Point GetCurrentAnimationFramePoint(GameTime gameTime)
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

    public abstract Rectangle GetHitBox(Vector2 position);
}
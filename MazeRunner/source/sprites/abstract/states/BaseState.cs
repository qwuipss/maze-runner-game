using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class BaseState : ISpriteState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public virtual SpriteEffects FrameEffect { get; set; }

    public virtual int FrameWidth
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return Texture.Height;
        }
    }

    public virtual int FrameAnimationDelayMs
    {
        get
        {
            return 50;
        }
    }

    public Point CurrentAnimationFramePoint
    {
        get
        {
            return new Point(CurrentAnimationFramePointX, 0);
        }
    }

    protected virtual int CurrentAnimationFramePointX { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public virtual ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs >= FrameAnimationDelayMs)
        {
            CurrentAnimationFramePointX = (CurrentAnimationFramePointX + FrameWidth) % (FrameWidth * FramesCount);
            ElapsedGameTimeMs -= FrameAnimationDelayMs;
        }

        return this;
    }
}
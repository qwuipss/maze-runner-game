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

    public virtual ISpriteState ProcessState()
    {
        CurrentAnimationFramePointX = (CurrentAnimationFramePointX + FrameWidth) % (FrameWidth * FramesCount);

        return this;
    }

    protected virtual int CurrentAnimationFramePointX { get; set; }
}
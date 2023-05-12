using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class SpriteBaseState : ISpriteState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public virtual SpriteEffects FrameEffect { get; set; }

    public virtual int UpdateTimeDelayMs
    {
        get
        {
            return 50;
        }
    }

    public virtual int FrameSize
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public Rectangle CurrentAnimationFrame
    {
        get
        {
            return new Rectangle(
                new Point(CurrentAnimationFramePointX, 0),
                new Point(FrameSize, FrameSize));
        }
    }

    protected virtual int CurrentAnimationFramePointX { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }
    
    public virtual ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            CurrentAnimationFramePointX = (CurrentAnimationFramePointX + FrameSize) % (FrameSize * FramesCount);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
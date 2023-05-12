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
            return new Rectangle(CurrentAnimationFramePoint, new Point(FrameSize, FrameSize));
        }
    }

    protected virtual Point CurrentAnimationFramePoint { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public static SpriteEffects ProcessFrameEffect(Vector2 movement, SpriteEffects frameEffect)
    {
        if (movement.X > 0)
        {
            return SpriteEffects.None;
        }
        else if (movement.X < 0)
        {
            return SpriteEffects.FlipHorizontally;
        }

        return frameEffect;
    }

    public virtual ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
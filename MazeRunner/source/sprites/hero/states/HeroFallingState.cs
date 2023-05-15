using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class HeroFallingState : HeroFallBaseState
{
    public HeroFallingState(ISpriteState previousState) : base(previousState)
    {
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 75;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new HeroFalledState(this);
            }

            var framePosX = animationPoint.X + FrameSize;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}

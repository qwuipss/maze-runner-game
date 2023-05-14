using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class HeroDyingState : HeroDeathBaseState
{
    public override int UpdateTimeDelayMs
    {
        get
        {
            return 100;
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
                return new HeroDeadState();
            }

            var framePosX = animationPoint.X + FrameSize;

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}

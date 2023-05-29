using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpeningState : ExitBaseState
{
    protected override double UpdateTimeDelayMs => 75;

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new ExitOpenedState();
            }

            animationPoint.X += FrameSize;

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatingState : DropTrapBaseState
{
    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            animationPoint.X += FrameSize;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new DropTrapActivatedState();
            }

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
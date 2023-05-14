using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatingState : BayonetTrapBaseState
{
    public BayonetTrapDeactivatingState()
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        var animationPoint = CurrentAnimationFramePoint;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            animationPoint.X -= FrameSize;

            if (animationPoint.X is 0)
            {
                return new BayonetTrapDeactivatedState();
            }

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
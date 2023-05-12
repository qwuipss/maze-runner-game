using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPostActivatingState : BayonetTrapBaseState
{
    public BayonetTrapPostActivatingState()
    {
        CurrentAnimationFramePoint = new Point(FrameSize, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            animationPoint.X += FrameSize;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new BayonetTrapActivatedState();
            }

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}

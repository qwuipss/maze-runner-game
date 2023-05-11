using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPostActivatingState : BayonetTrapBaseState
{
    public BayonetTrapPostActivatingState()
    {
        CurrentAnimationFramePointX = FrameSize;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            CurrentAnimationFramePointX += FrameSize;

            if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameSize)
            {
                return new BayonetTrapActivatedState();
            }

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}

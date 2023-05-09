using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatingState : BayonetTrapBaseState
{
    public BayonetTrapDeactivatingState()
    {
        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            CurrentAnimationFramePointX -= FrameWidth;

            if (CurrentAnimationFramePointX is 0)
            {
                return new BayonetTrapDeactivatedState();
            }

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}
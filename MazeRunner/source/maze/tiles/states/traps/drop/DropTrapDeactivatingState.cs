using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState()
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
                return new DropTrapDeactivatedState();
            }

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}
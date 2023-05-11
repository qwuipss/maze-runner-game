using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState()
    {
        CurrentAnimationFramePointX = (FramesCount - 1) * FrameSize;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            CurrentAnimationFramePointX -= FrameSize;

            if (CurrentAnimationFramePointX is 0)
            {
                return new DropTrapDeactivatedState();
            }

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}
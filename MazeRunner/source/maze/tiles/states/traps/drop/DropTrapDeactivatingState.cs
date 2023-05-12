using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState()
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            animationPoint.X -= FrameSize;

            if (animationPoint.X is 0)
            {
                return new DropTrapDeactivatedState();
            }

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}
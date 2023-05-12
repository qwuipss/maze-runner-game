using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatingState : DropTrapBaseState
{
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
                return new DropTrapActivatedState();
            }

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}
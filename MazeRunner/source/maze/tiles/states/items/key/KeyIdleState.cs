using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class KeyIdleState : KeyBaseState
{
    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}

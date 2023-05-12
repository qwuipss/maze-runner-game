using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpeningState : ExitBaseState
{
    protected override int UpdateTimeDelayMs
    {
        get
        {
            return 400;
        }
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var animationPoint = CurrentAnimationFramePoint;

        animationPoint.X += FrameSize;

        if (animationPoint.X == (FramesCount - 1) * FrameSize)
        {
            return new ExitOpenedState();
        }

        CurrentAnimationFramePoint = animationPoint;

        return this;
    }
}
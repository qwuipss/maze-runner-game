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
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new ExitOpenedState();
        }

        return this;
    }
}
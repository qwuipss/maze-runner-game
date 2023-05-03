namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpeningState : ExitBaseState
{
    public override int FrameAnimationDelayMs
    {
        get
        {
            return 400;
        }
    }

    public override IMazeTileState ProcessState()
    {
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new ExitOpenedState();
        }

        return this;
    }
}
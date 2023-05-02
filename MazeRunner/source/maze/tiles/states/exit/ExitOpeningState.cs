namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpeningState : BayonetTrapBaseState
{
    public ExitOpeningState()
    {
        CurrentAnimationFramePointX = FrameWidth;
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
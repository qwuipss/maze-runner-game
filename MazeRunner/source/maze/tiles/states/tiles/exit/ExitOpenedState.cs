namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpenedState : ExitBaseState
{
    public ExitOpenedState()
    {
        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        return this;
    }
}
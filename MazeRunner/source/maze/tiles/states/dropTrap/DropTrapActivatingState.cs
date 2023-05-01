namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatingState : DropTrapBaseState
{
    public DropTrapActivatingState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new DropTrapActivatedState(Trap);
        }

        return this;
    }
}
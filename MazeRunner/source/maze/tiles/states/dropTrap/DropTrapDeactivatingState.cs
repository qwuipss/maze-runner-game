namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 2) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        CurrentAnimationFramePointX -= FrameWidth;

        if (CurrentAnimationFramePointX is 0)
        {
            return new DropTrapDeactivatedState(Trap);
        }

        return this;
    }
}
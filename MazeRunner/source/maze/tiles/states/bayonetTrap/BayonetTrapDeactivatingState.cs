namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatingState : BayonetTrapBaseState
{
    public BayonetTrapDeactivatingState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        CurrentAnimationFramePointX -= FrameWidth;

        if (CurrentAnimationFramePointX is 0)
        {
            return new BayonetTrapDeactivatedState(Trap);
        }

        return this;
    }
}
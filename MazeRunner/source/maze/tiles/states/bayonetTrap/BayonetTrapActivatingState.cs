namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatingState : BayonetTrapBaseState
{
    public BayonetTrapActivatingState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new BayonetTrapActivatedState(Trap);
        }

        return this;
    }
}
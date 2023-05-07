using MazeRunner.Helpers;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatedState : DropTrapBaseState
{
    public DropTrapActivatedState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        if (RandomHelper.RollChance(Trap.DeactivateChance))
        {
            return new DropTrapDeactivatingState(Trap);
        }

        return this;
    }
}
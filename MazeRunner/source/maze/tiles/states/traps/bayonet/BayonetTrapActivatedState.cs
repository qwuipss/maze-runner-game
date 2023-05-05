#region Usings
using MazeRunner.Helpers;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatedState : BayonetTrapBaseState
{
    public BayonetTrapActivatedState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        if (RandomHelper.RollChance(Trap.DeactivateChance))
        {
            return new BayonetTrapDeactivatingState(Trap);
        }

        return this;
    }
}
using MazeRunner.Helpers;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatedState : BayonetTrapBaseState
{
    public BayonetTrapDeactivatedState(MazeTrap trap)
    {
        Trap = trap;
    }

    public override IMazeTileState ProcessState()
    {
        if (RandomHelper.RollChance(Trap.DeactivateChance))
        {
            return new BayonetTrapActivatingState(Trap);
        }

        return this;
    }
}
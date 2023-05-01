#region Usings
using MazeRunner.Helpers;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatedState : DropTrapBaseState
{
    public DropTrapDeactivatedState(MazeTrap trap)
    {
        Trap = trap;
    }

    public override IMazeTileState ProcessState()
    {
        if (RandomHelper.RollChance(Trap.ActivateChance))
        {
            return new DropTrapActivatingState(Trap);
        }

        return this;
    }
}
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatedState : DropTrapBaseState
{
    public DropTrapDeactivatedState(MazeTrap trap)
    {
        Trap = trap;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        if (RandomHelper.RollChance(Trap.ActivateChance))
        {
            return new DropTrapActivatingState(Trap);
        }

        return this;
    }
}
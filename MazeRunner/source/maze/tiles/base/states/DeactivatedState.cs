#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class DeactivatedState : IMazeTrapState
{
    private readonly MazeTrap _trap;

    public DeactivatedState(MazeTrap trap)
    {
        _trap = trap;
    }

    public Point CurrentAnimationPoint
    {
        get
        {
            return Point.Zero;
        }
    }

    public IMazeTrapState ProcessState()
    {
        if (RandomHelper.RollChance(_trap.DeactivateChance))
        {
            return new ActivatingState(_trap);
        }

        return this;
    }
}
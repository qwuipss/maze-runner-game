#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class ActivatedState : IMazeTrapState
{
    private readonly MazeTrap _trap;

    public ActivatedState(MazeTrap trap)
    {
        _trap = trap;
    }

    public Point CurrentAnimationPoint
    {
        get
        {
            return new Point((_trap.FramesCount - 1) * _trap.Width, 0);
        }
    }

    public IMazeTrapState ProcessState()
    {
        if (RandomHelper.RollChance(_trap.DeactivateChance))
        {
            return new DeactivatingState(_trap);
        }

        return this;
    }
}

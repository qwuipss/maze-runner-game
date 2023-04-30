#region Usings
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class ActivatedState : IMazeTrapState
{
    private readonly MazeTrap _trap;

    private readonly int _currentAnimationPointX;

    public ActivatedState(MazeTrap trap)
    {
        _trap = trap;

        _currentAnimationPointX = (_trap.FramesCount - 1) * _trap.Width;
    }

    public Point CurrentAnimationPoint
    {
        get
        {
            return new Point(_currentAnimationPointX, 0);
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

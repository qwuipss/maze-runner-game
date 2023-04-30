#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class ActivatingState : IMazeTrapState
{
    private readonly MazeTrap _trap;

    private int _currentAnimationPointX;

    public ActivatingState(MazeTrap trap)
    {
        _trap = trap;

        _currentAnimationPointX = _trap.Width;
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
        _currentAnimationPointX += _trap.Width;

        if (_currentAnimationPointX == (_trap.FramesCount - 1) * _trap.Width)
        {
            return new ActivatedState(_trap);
        }

        return this;
    }
}
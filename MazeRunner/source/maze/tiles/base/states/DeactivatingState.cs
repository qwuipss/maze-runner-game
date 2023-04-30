#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class DeactivatingState : IMazeTrapState
{
    private readonly MazeTrap _trap;

    private int _currentAnimationFrameX;

    public DeactivatingState(MazeTrap trap)
    {
        _trap = trap;

        _currentAnimationFrameX = (_trap.FramesCount - 2) * _trap.Width;
    }

    public Point CurrentAnimationPoint
    {
        get
        {
            return new Point(_currentAnimationFrameX, 0);
        }
    }

    public IMazeTrapState ProcessState()
    {
        _currentAnimationFrameX -= _trap.Width;

        if (_currentAnimationFrameX is 0)
        {
            return new DeactivatedState(_trap);
        }

        return this;
    }
}

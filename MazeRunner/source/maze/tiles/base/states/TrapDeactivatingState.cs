namespace MazeRunner;

public class TrapDeactivatingState : IMazeTrapState
{
    private readonly MazeTrap _mazeTrap;

    public TrapDeactivatingState(MazeTrap mazeTile)
    {
        _mazeTrap = mazeTile;
    }

    public IMazeTrapState ProcessState()
    {
        _mazeTrap.CurrentAnimationFrameX -= _mazeTrap.FrameWidth;

        if (_mazeTrap.CurrentAnimationFrameX is 0)
        {
            return new TrapDeactivatedState(_mazeTrap);
        }

        return this;
    }
}

namespace MazeRunner;

public class TrapActivatingState : IMazeTrapState
{
    private readonly MazeTrap _mazeTrap;

    public TrapActivatingState(MazeTrap mazeTile)
    {
        _mazeTrap = mazeTile;
    }

    public IMazeTrapState ProcessState()
    {
        _mazeTrap.CurrentAnimationFrameX += _mazeTrap.FrameWidth;

        if (_mazeTrap.CurrentAnimationFrameX == _mazeTrap.FrameWidth * _mazeTrap.FramesCount - _mazeTrap.FrameWidth)
        {
            return new TrapActivatedState(_mazeTrap);
        }

        return this;
    }
}
namespace MazeRunner;

public class TrapActivatedState : IMazeTrapState
{
    private readonly MazeTrap _mazeTrap;

    public TrapActivatedState(MazeTrap mazeTile)
    {
        _mazeTrap = mazeTile;
    }

    public IMazeTrapState ProcessState()
    {
        if (RandomHelper.Roll(_mazeTrap.DeactivateChance))
        {
            return new TrapDeactivatingState(_mazeTrap);
        }

        return this;
    }
}

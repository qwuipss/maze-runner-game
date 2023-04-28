namespace MazeRunner;

public class TrapDeactivatedState : IMazeTrapState
{
    private readonly MazeTrap _mazeTrap;

    public TrapDeactivatedState(MazeTrap mazeTile)
    {
        _mazeTrap = mazeTile;
    }

    public IMazeTrapState ProcessState()
    {
        if (RandomHelper.Roll(_mazeTrap.DeactivateChance))
        {
            return new TrapActivatingState(_mazeTrap);
        }

        return this;
    }
}
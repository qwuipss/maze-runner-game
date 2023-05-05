namespace MazeRunner.MazeBase.Tiles.States;

public class ExitClosedState : ExitBaseState
{
    public override IMazeTileState ProcessState()
    {
        return this;
    }
}
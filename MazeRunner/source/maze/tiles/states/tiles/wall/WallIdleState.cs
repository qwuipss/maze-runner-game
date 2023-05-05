namespace MazeRunner.MazeBase.Tiles.States;

public class WallIdleState : WallBaseState
{
    public override IMazeTileState ProcessState()
    {
        return this;
    }
}

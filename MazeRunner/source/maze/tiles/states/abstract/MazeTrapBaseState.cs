namespace MazeRunner.MazeBase.Tiles.States;

public abstract class MazeTrapBaseState : MazeTileBaseState
{
    protected virtual MazeTrap Trap { get; init; }
}
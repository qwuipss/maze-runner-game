namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public abstract TrapType TrapType { get; }

    public abstract bool IsActivated { get; }

    public override float DrawingPriority
    {
        get
        {
            return .8f;
        }
    }
}
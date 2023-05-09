namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public abstract TrapType TrapType { get; }

    public virtual bool IsActivated { get; }

    public virtual bool IsDeactivated { get; }

    public override float DrawingPriority
    {
        get
        {
            return .9f;
        }
    }
}
namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public abstract TrapType TrapType { get; }

    public abstract double ActivateChance { get; }

    public abstract double DeactivateChance { get; }

    public override float DrawingPriority
    {
        get
        {
            return .9f;
        }
    }
}
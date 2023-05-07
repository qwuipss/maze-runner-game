namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public override float DrawingPriority
    {
        get
        {
            return .9f;
        }
    }

    public abstract double ActivateChance { get; }

    public abstract double DeactivateChance { get; }
}
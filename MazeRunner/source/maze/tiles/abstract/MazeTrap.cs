namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public abstract double ActivateChance { get; }

    public abstract double DeactivateChance { get; }
}
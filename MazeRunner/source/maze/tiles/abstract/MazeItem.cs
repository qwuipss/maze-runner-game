namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeItem : MazeTile
{
    public abstract ItemType ItemType { get; }

    public override float DrawingPriority
    {
        get
        {
            return .8f;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Item;
        }
    }

    public virtual void ProcessCollecting(Maze maze, Cell cell)
    {
        maze.RemoveItem(cell);
    }
}
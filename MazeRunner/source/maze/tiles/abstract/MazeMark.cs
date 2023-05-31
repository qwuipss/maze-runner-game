namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeMark : MazeTile
{
    public override float DrawingPriority => .8f;

    public override TileType TileType => TileType.Mark;
}
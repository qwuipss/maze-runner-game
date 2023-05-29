using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class Wall : MazeTile
{
    public override TileType TileType => TileType.Wall;

    public override float DrawingPriority => .9f;

    public Wall()
    {
        State = new WallIdleState();
    }
}
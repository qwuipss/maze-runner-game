using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class Wall : MazeTile
{
    public override TileType TileType
    {
        get
        {
            return TileType.Wall;
        }
    }

    public Wall()
    {
        State = new WallIdleState();
    }
}
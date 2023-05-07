using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class Wall : MazeTile
{
    public Wall()
    {
        State = new WallIdleState();
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Wall;
        }
    }
}

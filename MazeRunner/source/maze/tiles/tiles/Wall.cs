#region Usings
using MazeRunner.MazeBase.Tiles.States;
#endregion

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

using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class Floor : MazeTile
{
    public override TileType TileType
    {
        get
        {
            return TileType.Floor;
        }
    }

    public Floor()
    {
        State = new FloorIdleState();
    }
}
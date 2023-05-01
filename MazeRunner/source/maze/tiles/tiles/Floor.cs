#region Usings
using MazeRunner.MazeBase.Tiles.States;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class Floor : MazeTile
{
    public Floor()
    {
        State = new FloorIdleState();
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Floor;
        }
    }
}

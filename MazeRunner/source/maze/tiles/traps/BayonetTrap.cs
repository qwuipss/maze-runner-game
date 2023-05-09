using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class BayonetTrap : MazeTrap
{
    public override TileType TileType
    {
        get
        {
            return TileType.Trap;
        }
    }

    public override TrapType TrapType
    {
        get
        {
            return TrapType.Bayonet;
        }
    }

    public BayonetTrap()
    {
        State = new BayonetTrapDeactivatedState();
    }
}
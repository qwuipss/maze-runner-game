using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

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

    public override double ActivateChance
    {
        get
        {
            return 1e-1 / 2;
        }
    }

    public override double DeactivateChance
    {
        get
        {
            return 1e-2 * 4;
        }
    }

    public BayonetTrap()
    {
        State = new BayonetTrapDeactivatedState(this);
    }
}
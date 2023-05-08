using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
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
            return TrapType.Drop;
        }
    }

    public override double ActivateChance
    {
        get
        {
            return 1e-2 / 3;
        }
    }

    public override double DeactivateChance
    {
        get
        {
            return 1e-1;
        }
    }

    public DropTrap()
    {
        State = new DropTrapDeactivatedState(this);
    }
}
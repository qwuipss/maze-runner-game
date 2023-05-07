using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class BayonetTrap : MazeTrap
{
    public override TileType TileType
    {
        get
        {
            return TileType.BayonetTrap;
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
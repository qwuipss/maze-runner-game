#region Usings
using MazeRunner.MazeBase.Tiles.States;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
{
    public DropTrap()
    {
        State = new DropTrapDeactivatedState(this);
    }

    public override TileType TileType
    {
        get
        {
            return TileType.DropTrap;
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
}
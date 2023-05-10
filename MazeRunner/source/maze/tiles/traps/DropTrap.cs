using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
{
    public override bool IsActivated
    {
        get
        {
            return State is DropTrapActivatedState;
        }
    }

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

    public DropTrap()
    {
        State = new DropTrapDeactivatedState();
    }
}
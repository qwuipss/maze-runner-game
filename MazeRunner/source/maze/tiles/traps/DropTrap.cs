using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
{
    private const int HitBoxOffset = 6;
    private const int HitBoxSize = 4;

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

    public override Rectangle GetHitBox(Vector2 position)
    {
        return new Rectangle(
            (int)position.X + HitBoxOffset,
            (int)position.Y + HitBoxOffset,
            HitBoxSize,
            HitBoxSize);
    }
}
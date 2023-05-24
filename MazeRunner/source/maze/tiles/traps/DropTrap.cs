using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
{
    private const float HitBoxOffset = 3;
    private const float HitBoxSize = 6;

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

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }
}
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class BayonetTrap : MazeTrap
{
    private const int HitBoxOffsetX = 1;
    private const int HitBoxOffsetY = 4;

    private const int HitBoxWidth = 14;
    private const int HitBoxHeight = 12;

    public override bool IsActivated
    {
        get
        {
            return State is BayonetTrapActivatedState || State is BayonetTrapPostActivatingState;
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
            return TrapType.Bayonet;
        }
    }

    public BayonetTrap()
    {
        State = new BayonetTrapDeactivatedState();
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }
}
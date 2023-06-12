using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class BayonetTrap : MazeTrap
{
    private const float HitBoxOffsetX = 1;

    private const float HitBoxOffsetY = 4;

    private const float HitBoxWidth = 14;

    private const float HitBoxHeight = 12;

    public override bool IsActivated => State is BayonetTrapActivatedState || State is BayonetTrapPostActivatingState;

    public override TileType TileType => TileType.Trap;

    public override TrapType TrapType => TrapType.Bayonet;

    public BayonetTrap(Hero hero)
    {
        State = new BayonetTrapDeactivatedState(hero, this);
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }
}
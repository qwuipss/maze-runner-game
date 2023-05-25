using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Guard : Enemy
{
    private const float HitBoxOffset = 5;

    private const float HitBoxSizeX = 5;
    private const float HitBoxSizeY = 11;

    private readonly int _halfHeartsDamage;

    public override bool IsDead
    {
        get
        {
            return State is GuardDeadState || State is GuardFalledState;
        }
    }

    public override int HalfHeartsDamage
    {
        get
        {
            return _halfHeartsDamage;
        }
    }

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(15, 15);
        }
    }

    public Guard(int halfHeartsDamage)
    {
        _halfHeartsDamage = halfHeartsDamage;
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSizeX, HitBoxSizeY);
    }

    public void Initialize(MazeRunnerGame game, SpriteInfo selfInfo)
    {
        State = new GuardIdleState(game.HeroInfo, selfInfo, game.MazeInfo);
    }
}

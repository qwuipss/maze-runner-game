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

    private float _drawingPriority;

    public override bool IsDead
    {
        get
        {
            return State is GuardDeadState || State is GuardFalledState;
        }
    }

    public override float DrawingPriority
    {
        get
        {
            return _drawingPriority;
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
        _drawingPriority = base.DrawingPriority;

        HalfHeartsDamage = halfHeartsDamage;
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSizeX, HitBoxSizeY);
    }

    public void Initialize(SpriteInfo selfInfo, SpriteInfo heroInfo, MazeInfo mazeInfo)
    {
        State = new GuardIdleState(heroInfo, selfInfo, mazeInfo);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_drawingPriority == base.DrawingPriority && IsDead)
        {
            _drawingPriority += .1f;
        }
    }
}

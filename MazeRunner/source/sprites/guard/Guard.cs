using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Guard : Enemy
{
    private const float HitBoxOffset = 5;

    private const float HitBoxSizeX = 5;

    private const float HitBoxSizeY = 11;

    private const float AttackDistanceCoeff = .85f;

    private float _drawingPriority;

    public override bool IsDead => State is GuardDeadState or GuardFalledState;

    public override float DrawingPriority => _drawingPriority;

    public override int Damage => 1;

    public override Vector2 Speed => new(15, 15);

    public Guard()
    {
        _drawingPriority = base.DrawingPriority;
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSizeX, HitBoxSizeY);
    }

    public void Initialize(Hero hero, Maze maze)
    {
        State = new GuardIdleState(hero, this, maze);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_drawingPriority == base.DrawingPriority && IsDead)
        {
            _drawingPriority += .1f;
        }
    }

    public override float GetAttackDistance()
    {
        return FrameSize * AttackDistanceCoeff;
    }
}

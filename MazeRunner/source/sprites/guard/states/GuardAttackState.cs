using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Sprites.States;

public class GuardAttackState : GuardBaseState
{
    private const double AttackDelayMs = 550;

    private bool _isAttacking;

    public static event Action AttackMissedNotify;

    public static event Action AttackHitNotify;

    public GuardAttackState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Attack;

    public override int FramesCount => 7;

    public override double UpdateTimeDelayMs => 100;

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (!_isAttacking && ElapsedGameTimeMs > AttackDelayMs)
        {
            _isAttacking = true;

            ElapsedGameTimeMs -= AttackDelayMs;
        }

        if (_isAttacking)
        {
            ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElapsedGameTimeMs > UpdateTimeDelayMs)
            {
                var animationPoint = CurrentAnimationFramePoint;

                if (animationPoint.X == (FramesCount - 1) * FrameSize)
                {
                    return new GuardIdleState(this, Hero, Guard, Maze);
                }

                var framePosX = animationPoint.X + FrameSize;

                CurrentAnimationFramePoint = new Point(framePosX, 0);
                ElapsedGameTimeMs -= UpdateTimeDelayMs;
            }

            DamageHeroIfNeeded();

            FaceToHero();
        }

        return this;
    }

    private void DamageHeroIfNeeded()
    {
        if (!Hero.IsDead && !Hero.IsTakingDamage)
        {
            if (Vector2.Distance(Hero.Position, Guard.Position) < Guard.ElongatedAttackDistance)
            {
                AttackHitNotify.Invoke();

                Hero.TakeDamage(Guard.Damage);
            }
            else
            {
                AttackMissedNotify.Invoke();
            }
        }
    }

    private void FaceToHero()
    {
        var turnDirectionX = (Hero.Position - Guard.Position).X;
        var materialMovement = new Vector2(turnDirectionX * float.Epsilon, 0);

        ProcessFrameEffect(materialMovement);
    }
}

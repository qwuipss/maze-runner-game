using MazeRunner.Content;
using MazeRunner.GameBase;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardAttackState : GuardBaseState
{
    private const double AttackDelayMs = 550;

    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly Maze _maze;

    private bool _isAttacking;

    public GuardAttackState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _maze = maze;
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Attack;

    public override int FramesCount => 7;

    public override double UpdateTimeDelayMs => 100;

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _maze, true, out var trapType))
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
                    return new GuardIdleState(this, _heroInfo, _guardInfo, _maze);
                }

                var framePosX = animationPoint.X + FrameSize;

                CurrentAnimationFramePoint = new Point(framePosX, 0);
                ElapsedGameTimeMs -= UpdateTimeDelayMs;
            }

            var hero = (Hero)_heroInfo.Sprite;

            if (!hero.IsDead
             && !hero.IsTakingDamage
             && Vector2.Distance(_heroInfo.Position, _guardInfo.Position) < Optimization.GetGuardAttackDistance(_guardInfo))
            {
                hero.TakeDamage();
            }

            FaceToHero();
        }

        return this;
    }

    private void FaceToHero()
    {
        var turnDirectionX = (_heroInfo.Position - _guardInfo.Position).X;
        var materialMovement = new Vector2(turnDirectionX * float.Epsilon, 0);

        ProcessFrameEffect(materialMovement);
    }
}

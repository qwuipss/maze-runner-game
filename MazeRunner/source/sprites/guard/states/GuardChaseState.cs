using MazeRunner.Managers;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private bool _isAttackOnCooldown;

    private double _cooldownTimeCounter;

    public GuardChaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze, bool isAttackOnCooldown, double cooldownTimeCounter = 0)
        : base(previousState, hero, guard, maze)
    {
        _isAttackOnCooldown = isAttackOnCooldown;
        _cooldownTimeCounter = cooldownTimeCounter;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        SoundManager.Sprites.Guard.ProcessRunSoundPlaying(Guard, GetDistanceToHero());

        if (_isAttackOnCooldown)
        {
            _cooldownTimeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_cooldownTimeCounter > GuardAttackState.AttackDelayMs)
            {
                _isAttackOnCooldown = false;
            }
        }

        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            SoundManager.Sprites.Guard.PlayTrapDeathSound(trapType, GetDistanceToHero());

            return GetTrapCollidingState(trapType);
        }

        if (!IsHeroNearby(out var pathToHero))
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        if (Vector2.Distance(Hero.Position, Guard.Position) < Guard.AttackDistance)
        {
            return new GuardAttackState(this, Hero, Guard, Maze, _isAttackOnCooldown, _cooldownTimeCounter);
        }

        var direction = GetMovementDirection(pathToHero);

        if (!ProcessMovement(direction, gameTime))
        {
            return new GuardChaseAwaitState(this, Hero, Guard, Maze, _isAttackOnCooldown, _cooldownTimeCounter);
        }

        if (CollidesWithTraps(Guard, Maze, true, out trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        base.ProcessState(gameTime);

        return this;
    }
}
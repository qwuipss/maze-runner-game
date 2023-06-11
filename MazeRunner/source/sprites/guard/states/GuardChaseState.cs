using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private bool _isAttackOnCooldown;

    private double _elapsedTimeCounter;

    public GuardChaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze, bool isAttackOnCooldown = true) 
        : base(previousState, hero, guard, maze)
    {
        _isAttackOnCooldown = isAttackOnCooldown;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (_isAttackOnCooldown)
        {
            _elapsedTimeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTimeCounter > GuardAttackState.AttackDelayMs)
            {
                _isAttackOnCooldown = false;
            }
        }

        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (!IsHeroNearby(out var pathToHero))
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        if (Vector2.Distance(Hero.Position, Guard.Position) < Guard.AttackDistance)
        {
            return new GuardAttackState(this, Hero, Guard, Maze, _isAttackOnCooldown);
        }

        var direction = GetMovementDirection(pathToHero);

        if (!ProcessMovement(direction, gameTime))
        {
            return new GuardChaseAwaitState(this, Hero, Guard, Maze);
        }

        base.ProcessState(gameTime);

        return this;
    }
}
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private const float GuardAttackDistanceCoeff = .85f;

    public GuardChaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (!IsHeroNearby(out var pathToHero))
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        if (Vector2.Distance(Hero.Position, Guard.Position) < Guard.GetAttackDistance())
        {
            return new GuardAttackState(this, Hero, Guard, Maze);
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
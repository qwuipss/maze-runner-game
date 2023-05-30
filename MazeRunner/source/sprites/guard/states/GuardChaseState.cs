using MazeRunner.GameBase;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
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

        if (CanAttack(Hero, Guard))
        {
            return new GuardAttackState(this, Hero, Guard, Maze);
        }

        var direction = GetMovementDirection(Guard, pathToHero);

        if (!ProcessMovement(Guard, direction, Maze, gameTime))
        {
            return new GuardChaseAwaitState(this, Hero, Guard, Maze);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private static bool CanAttack(Hero hero, Guard guard)
    {
        var distance = Vector2.Distance(hero.Position, guard.Position);

        return distance < Optimization.GetGuardAttackDistance(guard);
    }
}

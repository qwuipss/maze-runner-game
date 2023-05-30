using MazeRunner.GameBase;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly Maze _maze;

    public GuardChaseState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _maze = maze;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (!IsHeroNearby(_heroInfo, _guardInfo, _maze, out var pathToHero))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _maze);
        }

        if (CanAttack(_heroInfo, _guardInfo))
        {
            return new GuardAttackState(this, _heroInfo, _guardInfo, _maze);
        }

        var direction = GetMovementDirection(_guardInfo, pathToHero);

        if (!ProcessMovement(_guardInfo, direction, _maze, gameTime))
        {
            return new GuardChaseAwaitState(this, _heroInfo, _guardInfo, _maze);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private static bool CanAttack(SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        var distance = Vector2.Distance(heroInfo.Position, guardInfo.Position);

        return distance < Optimization.GetGuardAttackDistance(guardInfo);
    }
}

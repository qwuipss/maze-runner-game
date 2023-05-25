using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;
    private readonly SpriteInfo _guardInfo;

    private readonly MazeInfo _mazeInfo;

    public GuardChaseState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _mazeInfo = mazeInfo;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (!IsHeroNearby(_heroInfo, _guardInfo, _mazeInfo, out var pathToHero))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (CanAttack(_heroInfo, _guardInfo))
        {
            return new GuardAttackState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var direction = GetMovementDirection(_guardInfo, pathToHero);

        if (!ProcessMovement(_guardInfo, direction, _mazeInfo.Maze, gameTime))
        {
            return new GuardChaseAwaitState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private static bool CanAttack(SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        var distance = Vector2.Distance(heroInfo.Position, guardInfo.Position);

        return distance < guardInfo.Sprite.FrameSize * OptimizationConstants.GuardAttackDistanceCoeff;
    }
}

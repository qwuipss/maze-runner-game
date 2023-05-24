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
        if (!IsHeroNearby(_heroInfo, _guardInfo))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (CollidesWithTraps(_guardInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (CanAttack(_heroInfo, _guardInfo))
        {
            return new GuardAttackState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var direction = GetMovementDirection(_heroInfo, _guardInfo, _mazeInfo);

        if (direction == Vector2.Zero)
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (!ProcessMovement(_guardInfo, direction, _mazeInfo.Maze, gameTime))
        {
            return new GuardChaseAwaitState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private static bool CanAttack(SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        const float attackDistanceCoeff = .8f;

        var distance = Vector2.Distance(heroInfo.Position, guardInfo.Position);

        return distance < guardInfo.Sprite.FrameSize * attackDistanceCoeff;
    }
}

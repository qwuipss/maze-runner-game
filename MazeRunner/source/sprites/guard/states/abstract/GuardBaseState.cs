using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public abstract class GuardBaseState : SpriteBaseState
{
    protected GuardBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    protected static bool IsHeroNearby(SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        const float detectionDistanceCoeff = 3.5f;

        var distance = Vector2.Distance(heroInfo.Position, guardInfo.Position);

        return distance <= guardInfo.Sprite.FrameSize * detectionDistanceCoeff;
    }

    protected override GuardBaseState GetTrapCollidingState(TrapType trapType)
    {
        return this;
    }
}
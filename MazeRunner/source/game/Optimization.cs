using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;

namespace MazeRunner.GameBase;

internal static class Optimization
{
    internal const int GuardHeroMaxPathLength = 7;

    private const float EnemySpawnDistanceCoeff = 3;
    private const float EnemyDisposingDistanceCoeff = 7;
    private const float EnemiesUpdateDistanceCoeff = 10;

    private const float GuardAttackDistanceCoeff = .85f;
    private const float GuardHeroDetectionDistanceCoeff = 3;

    private const float MazeTileCollidingDistanceCoeff = 1.5f;
    private const float MazeTileUpdateDistanceCoeff = 7;

    public static float GetGuardHeroDetectionDistance(SpriteInfo guardInfo)
    {
        return guardInfo.Sprite.FrameSize * GuardHeroDetectionDistanceCoeff;
    }

    public static float GetGuardAttackDistance(SpriteInfo guardInfo)
    {
        return guardInfo.Sprite.FrameSize * GuardAttackDistanceCoeff;
    }

    public static float GetMazeTileUpdateDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * MazeTileUpdateDistanceCoeff;
    }

    public static float GetMazeTileCollidingCheckDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * MazeTileCollidingDistanceCoeff;
    }

    public static float GetEnemyUpdateDistance(SpriteInfo spriteInfo)
    {
        return spriteInfo.Sprite.FrameSize * EnemiesUpdateDistanceCoeff;
    }

    public static float GetEnemyDisposingDistance(SpriteInfo spriteInfo)
    {
        return spriteInfo.Sprite.FrameSize * EnemyDisposingDistanceCoeff;
    }

    public static float GetEnemySpawnDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * EnemySpawnDistanceCoeff;
    }
}
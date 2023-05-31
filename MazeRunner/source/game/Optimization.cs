using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;

namespace MazeRunner.GameBase;

internal static class Optimization
{
    internal const int GuardHeroMaxPathLength = 7;

    private const float EnemySpawnDistanceCoeff = 3;
    private const float EnemyDisposingDistanceCoeff = 3; // 7
    private const float EnemiesUpdateDistanceCoeff = 10;

    private const float GuardHeroDetectionDistanceCoeff = 3;

    private const float MazeTileCollidingDistanceCoeff = 1.5f;
    private const float MazeTileUpdateDistanceCoeff = 7;

    public static float GetGuardHeroDetectionDistance(Guard guard)
    {
        return guard.FrameSize * GuardHeroDetectionDistanceCoeff;
    }

    public static float GetMazeTileUpdateDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * MazeTileUpdateDistanceCoeff;
    }

    public static float GetMazeTileCollidingCheckDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * MazeTileCollidingDistanceCoeff;
    }

    public static float GetEnemyUpdateDistance(Enemy enemy)
    {
        return enemy.FrameSize * EnemiesUpdateDistanceCoeff;
    }

    public static float GetEnemyDisposingDistance(Enemy enemy)
    {
        return enemy.FrameSize * EnemyDisposingDistanceCoeff;
    }

    public static float GetEnemySpawnDistance(MazeTile mazeTile)
    {
        return mazeTile.FrameSize * EnemySpawnDistanceCoeff;
    }
}
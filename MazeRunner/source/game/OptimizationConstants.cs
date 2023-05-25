namespace MazeRunner;

internal static class OptimizationConstants
{
    internal const float EnemySpawnDistanceCoeff = 3;
    internal const float GuardAttackDistanceCoeff = .85f;
    internal const float GuardHeroDetectionDistanceCoeff = 3;
    internal const float GuardDisposingDistanceCoeff = 7;
    internal const float EnemiesUpdateDistanceCoeff = 10;
    internal const float MazeTileCollidingDistanceCoeff = 1.5f;
    internal const float MazeTileUpdateDistanceCoeff = 7;

    internal const int GuardHeroMaxPathLength = 7;
}

using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;

namespace MazeRunner.GameBase;

internal static class Optimization
{
    internal const int GuardHeroMaxPathLength = 7;

    private const float EnemySpawnDistanceCoeff = 3;
    private const float EnemyDisposingDistanceCoeff = 7;
    private const float EnemiesUpdateDistanceCoeff = 10;

    private const float GuardHeroDetectionDistanceCoeff = 3;

    private const float MazeTileCollidingDistanceCoeff = 1.5f;
    private const float MazeTileUpdateDistanceCoeff = 7;

    public const float GuardHeroDetectionDistance = GameConstants.AssetsFrameSize * GuardHeroDetectionDistanceCoeff;

    public const float MazeTileUpdateDistance = GameConstants.AssetsFrameSize * MazeTileUpdateDistanceCoeff;
    public const float MazeTileCollidingCheckDistance = GameConstants.AssetsFrameSize * MazeTileCollidingDistanceCoeff;

    public const float EnemyUpdateDistance = GameConstants.AssetsFrameSize * EnemiesUpdateDistanceCoeff;
    public const float EnemyDisposingDistance = GameConstants.AssetsFrameSize * EnemyDisposingDistanceCoeff;
    public const float EnemySpawnDistance = GameConstants.AssetsFrameSize * EnemySpawnDistanceCoeff;
}